# CSRF (Cross-Site Request Forgery, Anti Forgery)

CSRF (クロスサイトリクエストフォージェリー、) は、信頼されたユーザーになりすまし、ウェブサイトに対して不正なコマンドを送信する攻撃です。

"X-Requested-With": "XMLHttpRequest",
"X-CSRF-Token": csrf_token
