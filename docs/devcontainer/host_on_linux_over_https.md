# Host on MacOS Over HTTPS

## Environments

- Fedora 34 (5.13.8-200.fc34.x86_64)
- openssl-1.1.1k-1.fc34.x86_64
- ca-certificates-2021.2.50-1.0.fc34.noarch
- p11-kit-trust-0.23.22-3.fc34.x86_64

## Create Self-signed Certificate

### Use OpenSSL

ディレクトリを用意します。

```
mkdir -p ~/.dotnet/https
cd ~/.dotnet/https
```

[Micorosoft のサイト](https://docs.microsoft.com/ja-jp/dotnet/core/additional-tools/self-signed-certificates-guide#with-openssl)のコマンドで.crt と .key 作成します。

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

.crt の内容は次のコマンドで確認します。

```shell
openssl x509 -noout -text -in $PARENT.crt
```

.pfx を作成します。
ここでパスワードフレーズが必要になります。

```shell
openssl pkcs12 -export -out $PARENT.pfx -inkey $PARENT.key -in $PARENT.crt
```

CA 証明書を承認しますします。

Linux ディストリビューションによって方法が違うようです

- [参考]{https://qiita.com/msi/items/9cb90271836386dafce3)

```shell
sudo cp localhost.crt /usr/share/pki/ca-trust-source/anchors/
sudo update-ca-trust
```

次のコマンドで確認できます。

```shell
trust list
```

証明証の削除も `trust` コマンドで行うようです。

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
