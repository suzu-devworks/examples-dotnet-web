#!/bin/sh
# spell-checker: disable
echo "Generating OpenSSL files..."
openssl version

FILE_DIR=$(dirname "$0")
TARGET_DIR="${1:-./assets}"
mkdir -p "$TARGET_DIR"
DEST_DIR=$(cd "$TARGET_DIR" && pwd)
echo "Output To: ${DEST_DIR}"

CONF_FILE=${FILE_DIR}/openssl.cnf

DAYS=3
echo "Certificate valid for ${DAYS} days"

# Generate a random password file
head -c 500 /dev/urandom | LC_CTYPE=C tr -dc 'a-zA-Z0-9!@#\$%&/:;\^()_+\-=<>?' | head -c 24 > ${DEST_DIR}/.password
chmod 600 "${DEST_DIR}/.password"
wc -c < ${DEST_DIR}/.password | awk '{print "Password length: " $1}'
echo ""

# Self signed root CA (ECDSA)
openssl ecparam -genkey -name secp384r1 -noout -out ${DEST_DIR}/example.ca-root.key
openssl req -new -x509 -config ${CONF_FILE} -batch \
    -subj "/C=JP/O=examples/CN=Example Root CA" \
    -key ${DEST_DIR}/example.ca-root.key \
    -out ${DEST_DIR}/example.ca-root.crt -days ${DAYS} -extensions v3_ca

# JWT signer certs (ECDSA)
openssl ecparam -genkey -name prime256v1 -noout -out ${DEST_DIR}/example.signer.key
openssl req -new -config ${CONF_FILE} -batch \
    -subj "/C=JP/CN=*.ecdsa.example.com" \
    -key ${DEST_DIR}/example.signer.key \
    -out ${DEST_DIR}/example.signer.csr
openssl x509 -req -in ${DEST_DIR}/example.signer.csr \
    -CA ${DEST_DIR}/example.ca-root.crt -CAkey ${DEST_DIR}/example.ca-root.key -CAcreateserial \
    -extfile ${CONF_FILE} -extensions v3_cert \
    -out ${DEST_DIR}/example.signer.crt -days ${DAYS}
openssl pkcs12 -export -in ${DEST_DIR}/example.signer.crt -inkey ${DEST_DIR}/example.signer.key \
    -out ${DEST_DIR}/example.signer.p12 -passout file:${DEST_DIR}/.password

# Client cert (RSA)
openssl genrsa -traditional -out ${DEST_DIR}/example-user.rsa.key 4096
openssl req -new -config ${CONF_FILE} -batch \
    -subj "/CN=user@example.com" \
    -key ${DEST_DIR}/example-user.rsa.key \
    -out ${DEST_DIR}/example-user.rsa.csr
openssl x509 -req -in ${DEST_DIR}/example-user.rsa.csr \
    -CA ${DEST_DIR}/example.ca-root.crt -CAkey ${DEST_DIR}/example.ca-root.key -CAcreateserial \
    -extfile ${CONF_FILE} -extensions v3_client \
    -out ${DEST_DIR}/example-user.rsa.crt -days ${DAYS}
openssl pkcs12 -export -in ${DEST_DIR}/example-user.rsa.crt -inkey ${DEST_DIR}/example-user.rsa.key \
    -out ${DEST_DIR}/example-user.rsa.p12 -passout file:${DEST_DIR}/.password

# Client cert (ECDSA)
openssl ecparam -genkey -name prime256v1 -noout -out ${DEST_DIR}/example-user.ecdsa.key
openssl req -new -config ${CONF_FILE} -batch \
    -subj "/CN=user@example.com" \
    -key ${DEST_DIR}/example-user.ecdsa.key \
    -out ${DEST_DIR}/example-user.ecdsa.csr
openssl x509 -req -in ${DEST_DIR}/example-user.ecdsa.csr \
    -CA ${DEST_DIR}/example.ca-root.crt -CAkey ${DEST_DIR}/example.ca-root.key -CAcreateserial \
    -extfile ${CONF_FILE} -extensions v3_client \
    -out ${DEST_DIR}/example-user.ecdsa.crt -days ${DAYS}
openssl pkcs12 -export -in ${DEST_DIR}/example-user.ecdsa.crt -inkey ${DEST_DIR}/example-user.ecdsa.key \
    -out ${DEST_DIR}/example-user.ecdsa.p12 -passout file:${DEST_DIR}/.password

if [ $? -eq 0 ]; then
    rm -f ${DEST_DIR}/example.*.csr
    rm -f ${DEST_DIR}/example-user.*.csr
fi

echo ""
echo "OpenSSL files generated."
ls -l ${DEST_DIR}/example*
