#!/bin/bash

BASE_DIR=$(cd $(dirname "$0"); pwd)
CONF_FILE="$BASE_DIR/ssl.conf"
DEST_DIR="$BASE_DIR"
ENV_FILE="$BASE_DIR/../../.env"

# Generate a self-signed SSL certificate for localhost
openssl ecparam -genkey -name prime256v1 -noout -out "$DEST_DIR/localhost.key"
openssl req -new -x509 -config $CONF_FILE -key "$DEST_DIR/localhost.key" -sha256 -days 365 -out "$DEST_DIR/localhost.crt" -subj="/CN=localhost"

# Generate a random password for the SSL certificate
NEW_PASS=$(cat /dev/urandom | LC_CTYPE=C tr -dc 'a-zA-Z0-9!@#\$%&/:;\^()_+\-=<>?' | fold -w 24 | head -n 1)

# write the password to .env file
if grep -q "KESTREL_CERT_PASSWORD=" "$ENV_FILE"; then
    # 既存の行を置換
    sed -i "s/^KESTREL_CERT_PASSWORD=.*/KESTREL_CERT_PASSWORD=\"${NEW_PASS}\"/" "$ENV_FILE"
else
    # ファイルの末尾に追記
    echo "KESTREL_CERT_PASSWORD=\"${NEW_PASS}\"" >> "$ENV_FILE"
fi

# Create a PKCS#12 file containing the certificate and private key
openssl pkcs12 -export -out "$DEST_DIR/localhost.pfx" -inkey "$DEST_DIR/localhost.key" -in "$DEST_DIR/localhost.crt" -passout pass:$NEW_PASS

echo "SSL certificate generated successfully."