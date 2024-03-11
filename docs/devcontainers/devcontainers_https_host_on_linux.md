# Dev Containers HTTPS host on Linux

## Table of Contents <!-- omit in toc -->

- [Dev Containers HTTPS host on Linux](#dev-containers-https-host-on-linux)
  - [Environments](#environments)
  - [Create Self-signed Certificate](#create-self-signed-certificate)
    - [Use OpenSSL](#use-openssl)
  - [TLS 1.3 to be used by default in environments that support it.](#tls-13-to-be-used-by-default-in-environments-that-support-it)


## Environments

- Fedora 34 (5.13.8-200.fc34.x86_64)
- openssl-1.1.1k-1.fc34.x86_64
- ca-certificates-2021.2.50-1.0.fc34.noarch
- p11-kit-trust-0.23.22-3markdown.fc34.x86_64

## Create Self-signed Certificate

### Use OpenSSL

Prepare a directory:

```shell
mkdir -p ~/.dotnet/https
cd ~/.dotnet/https
```

Create `.crt` and `.key` files using the command on the [Micorosoft site](https://docs.microsoft.com/ja-jp/dotnet/core/additional-tools/self-signed-certificates-guide#with-openssl).

```shell
PARENT="localhost"
openssl req \
-x509 \
-newkey rsa:4096 \
-sha256 \
-days 365 \
-nodes \
-keyout $PARENT.key \
-out $PARENT.crt \
-subj "/CN=${PARENT}" \
-extensions v3_ca \
-extensions v3_req \
-config <( \
  echo '[req]'; \
  echo 'default_bits= 4096'; \
  echo 'distinguished_name=req'; \
  echo 'x509_extension = v3_ca'; \
  echo 'req_extensions = v3_req'; \
  echo '[v3_req]'; \
  echo 'basicConstraints = CA:FALSE'; \
  echo 'keyUsage = nonRepudiation, digitalSignature, keyEncipherment'; \
  echo 'subjectAltName = @alt_names'; \
  echo '[ alt_names ]'; \
  echo "DNS.1 = ${PARENT}"; \
  echo "IP.1 = 127.0.0.1"; \
  echo '[ v3_ca ]'; \
  echo 'subjectKeyIdentifier=hash'; \
  echo 'authorityKeyIdentifier=keyid:always,issuer'; \
  echo 'basicConstraints = critical, CA:TRUE, pathlen:0'; \
  echo 'keyUsage = critical, cRLSign, keyCertSign'; \
  echo 'extendedKeyUsage = serverAuth, clientAuth')
```

Verify the contents of the `.crt` file with the following command:

```shell
openssl x509 -noout -text -in $PARENT.crt
```

Create a PKCS12 `.pfx`:

```shell
openssl pkcs12 -export -out $PARENT.pfx -inkey $PARENT.key -in $PARENT.crt

# You will need your password phrase here.

```

Approve CA certificate. 

The method seems to be different depending on the Linux distribution.
The following commands are from the Red Hat family:

```shell
sudo cp localhost.crt /usr/share/pki/ca-trust-source/anchors/
sudo update-ca-trust
```

You can check it with the following command:

```shell
trust list
```

It seems that certificates can also be deleted using the `trust` command.


## TLS 1.3 to be used by default in environments that support it.

```
08:22:11.323+00:00|EventId_Id=1, EventId_Name=AuthenticationFailed, EventId=AuthenticationFailed System.IO.IOException: Cannot determine the frame size or a corrupted frame was received.
   at System.Net.Security.SslStream.ReceiveBlobAsync[TIOAdapter](TIOAdapter adapter)
   at System.Net.Security.SslStream.ForceAuthenticationAsync[TIOAdapter](TIOAdapter adapter, Boolean receiveFirst, Byte[] reAuthenticationData, Boolean isApm)
   at Microsoft.AspNetCore.Server.Kestrel.Https.Internal.HttpsConnectionMiddleware.OnConnectionAsync(ConnectionContext context)
```

```console
openssl ciphers -v TLSv1.3
Error in cipher list
139926610777920:error:1410D0B9:SSL routines:SSL_CTX_set_cipher_list:no cipher match:ssl/ssl_lib.c:2566:
```
