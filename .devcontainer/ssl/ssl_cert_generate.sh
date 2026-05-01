#!/bin/sh
# spell-checker: disable
echo "Generating OpenSSL files..."
openssl version

BASE_DIR=$(cd $(dirname "$0"); pwd)
DEST_DIR="${1:-${BASE_DIR}}"

mkdir -p "$DEST_DIR"
echo "Output to: ${DEST_DIR}"

CONF_FILE=${BASE_DIR}/ssl.conf

echo "Using OpenSSL config: ${CONF_FILE}"

DAYS=30
echo "Certificate valid for: ${DAYS} days"

# Generate a random password file
PASSWORD_FILE="${DEST_DIR}/.password"
head -c 500 /dev/urandom | LC_CTYPE=C tr -dc 'a-zA-Z0-9!@#\$%&/:;\^()_+\-=<>?' | head -c 24 > "${PASSWORD_FILE}"
chmod 600 "${PASSWORD_FILE}"
wc -c < "${PASSWORD_FILE}" | awk '{print "Password length: " $1}'
echo ""

# Generate a self-signed SSL certificate for external localhost
openssl ecparam -genkey -name prime256v1 -noout -out "$DEST_DIR/localhost.key"
openssl req -new -x509 -config ${CONF_FILE} -batch \
    -subj="/CN=examples-dotnet-web" \
    -key ${DEST_DIR}/localhost.key \
    -out ${DEST_DIR}/localhost.crt -days ${DAYS}
openssl pkcs12 -export -in ${DEST_DIR}/localhost.crt -inkey ${DEST_DIR}/localhost.key \
    -out ${DEST_DIR}/localhost.pfx -passout file:${PASSWORD_FILE}

echo "SSL certificate generated successfully."

if [ $? -eq 0 ]; then
    rm -f ${DEST_DIR}/*.csr
fi

echo ""
echo "OpenSSL files generated."
ls -l ${DEST_DIR}/*
