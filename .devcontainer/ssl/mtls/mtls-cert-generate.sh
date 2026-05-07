#!/usr/bin/env bash
# spell-checker: disable
echo "Generating OpenSSL files..."
openssl version

BASE_DIR=$(cd $(dirname "$0"); pwd)
DEST_DIR="${1:-${BASE_DIR}}"

mkdir -p "$DEST_DIR"
echo "Output to: ${DEST_DIR}"

CONF_FILE=${BASE_DIR}/mtls.conf
echo "Using OpenSSL config: ${CONF_FILE}"

DAYS=30
echo "Certificate valid for: ${DAYS} days"

# Generate a random password file
PASSWORD_FILE="${DEST_DIR}/.password"
head -c 500 /dev/urandom | LC_CTYPE=C tr -dc 'a-zA-Z0-9!@#\$%&/:;\^()_+\-=<>?' | head -c 24 > "${PASSWORD_FILE}"
chmod 600 "${PASSWORD_FILE}"
wc -c < "${PASSWORD_FILE}" | awk '{print "Password length: " $1}'
echo ""

# Backend mTLS certificates for development environment

# Generate a self-signed mTLS CA certificate for internal use
openssl ecparam -genkey -name prime256v1 -noout -out "$DEST_DIR/internal-ca.key"
openssl req -new -x509 -config $CONF_FILE -key "$DEST_DIR/internal-ca.key" -sha256 -days ${DAYS} -out "$DEST_DIR/internal-ca.crt" -subj="/CN=internal mTLS CA"

# Generate a client certificate for mTLS authentication on the web server
openssl ecparam -genkey -name prime256v1 -noout -out ${DEST_DIR}/internal-web.key
openssl req -new -config ${CONF_FILE} -batch \
    -subj "/CN=web" \
    -key ${DEST_DIR}/internal-web.key \
    -out ${DEST_DIR}/internal-web.csr
openssl x509 -req -in ${DEST_DIR}/internal-web.csr \
    -CA ${DEST_DIR}/internal-ca.crt -CAkey ${DEST_DIR}/internal-ca.key -CAcreateserial \
    -extfile ${CONF_FILE} -extensions v3_web_client \
    -out ${DEST_DIR}/internal-web.crt -days ${DAYS}

# Generate a server certificate for mTLS authentication on the dev server
openssl ecparam -genkey -name prime256v1 -noout -out ${DEST_DIR}/internal-dev.key
openssl req -new -config ${CONF_FILE} -batch \
    -subj "/CN=dev" \
    -key ${DEST_DIR}/internal-dev.key \
    -out ${DEST_DIR}/internal-dev.csr
openssl x509 -req -in ${DEST_DIR}/internal-dev.csr \
    -CA ${DEST_DIR}/internal-ca.crt -CAkey ${DEST_DIR}/internal-ca.key -CAcreateserial \
    -extfile ${CONF_FILE} -extensions v3_dev_server \
    -out ${DEST_DIR}/internal-dev.crt -days ${DAYS}
openssl pkcs12 -export -in ${DEST_DIR}/internal-dev.crt -inkey ${DEST_DIR}/internal-dev.key \
    -out ${DEST_DIR}/internal-dev.pfx -passout file:${PASSWORD_FILE}

# Frontend mTLS certificates for external use

# Generate a self-signed mTLS CA certificate for external use
openssl ecparam -genkey -name prime256v1 -noout -out "$DEST_DIR/external-ca.key"
openssl req -new -x509 -config $CONF_FILE -key "$DEST_DIR/external-ca.key" -sha256 -days ${DAYS} -out "$DEST_DIR/external-ca.crt" -subj="/CN=external mTLS CA"

# Generate a client certificate for mTLS authentication on the web server
openssl ecparam -genkey -name prime256v1 -noout -out ${DEST_DIR}/external-client.key
openssl req -new -config ${CONF_FILE} -batch \
    -subj "/CN=external-client" \
    -key ${DEST_DIR}/external-client.key \
    -out ${DEST_DIR}/external-client.csr
openssl x509 -req -in ${DEST_DIR}/external-client.csr \
    -CA ${DEST_DIR}/external-ca.crt -CAkey ${DEST_DIR}/external-ca.key -CAcreateserial \
    -extfile ${CONF_FILE} -extensions v3_client \
    -out ${DEST_DIR}/external-client.crt -days ${DAYS}
openssl pkcs12 -export -in ${DEST_DIR}/external-client.crt -inkey ${DEST_DIR}/external-client.key \
    -out ${DEST_DIR}/external-client.pfx -passout file:${PASSWORD_FILE}

if [ $? -eq 0 ]; then
    rm -f ${DEST_DIR}/*.csr
fi

echo "mTLS certificates generated successfully."

echo ""
echo "OpenSSL files generated."
ls -l ${DEST_DIR}/*.{crt,key,pfx}
