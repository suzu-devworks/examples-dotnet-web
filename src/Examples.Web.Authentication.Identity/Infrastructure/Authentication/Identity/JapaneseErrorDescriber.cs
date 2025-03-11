using Microsoft.AspNetCore.Identity;

namespace Examples.Web.Infrastructure.Authentication.Identity;

public class JapaneseErrorDescriber : IdentityErrorDescriber
{

    public override IdentityError ConcurrencyFailure() => new()
    {
        Code = nameof(ConcurrencyFailure),
        // Description = "Optimistic concurrency failure, object has been modified."
        Description = "オプティミスティック同時実行の失敗、オブジェクトが変更されました。"
    };

    public override IdentityError DefaultError() => new()
    {
        Code = nameof(DefaultError),
        // Description = $"An unknown failure has occurred."
        Description = "不明な障害が発生しました。"
    };

    public override IdentityError DuplicateEmail(string email) => new()
    {
        Code = nameof(DuplicateEmail),
        // Description = $"Email '{email}' is already taken."
        Description = $"メール「{email}」は既に使用されています。"
    };

    public override IdentityError DuplicateRoleName(string role) => new()
    {
        Code = nameof(DuplicateRoleName),
        // Description = $"Role name '{role}' is already taken."
        Description = $"ロール名「{role}」は既に使用されています。"
    };

    public override IdentityError DuplicateUserName(string userName) => new()
    {
        Code = nameof(DuplicateUserName),
        // Description = $"User Name '{userName}' is already taken."
        Description = $"ユーザー名「{userName}」は既に使用されています。"
    };

    public override IdentityError InvalidEmail(string? email) => new()
    {
        Code = nameof(InvalidEmail),
        // Description = $"Email '{email}' is invalid."
        Description = $"メール「{email}」は無効です。"
    };

    public override IdentityError InvalidRoleName(string? role) => new()
    {
        Code = nameof(InvalidRoleName),
        // Description = $"Role name '{role}' is invalid."
        Description = $"ロール名「{role}」は無効です。"
    };

    public override IdentityError InvalidToken() => new()
    {
        Code = nameof(InvalidToken),
        // Description = "Invalid token."
        Description = "トークンが無効です。"
    };

    public override IdentityError InvalidUserName(string? userName) => new()
    {
        Code = nameof(InvalidUserName),
        // Description = $"User name '{userName}' is invalid, can only contain letters or digits."
        Description = $"ユーザー名「{userName}」は無効です。文字または数字のみを含めることができます。"
    };

    public override IdentityError LoginAlreadyAssociated() => new()
    {
        Code = nameof(LoginAlreadyAssociated),
        // Description = "A user with this login already exists."
        Description = "このログインを持つユーザーはすでに存在します。"
    };

    public override IdentityError PasswordMismatch() => new()
    {
        Code = nameof(PasswordMismatch),
        // Description = "Incorrect password."
        Description = "パスワードが間違っています。"
    };

    public override IdentityError PasswordRequiresDigit() => new()
    {
        Code = nameof(PasswordRequiresDigit),
        // Description = "Passwords must have at least one digit ('0'-'9')."
        Description = "パスワードには少なくとも 1 つの数字 ('0'-'9') が必要です。"
    };

    public override IdentityError PasswordRequiresLower() => new()
    {
        Code = nameof(PasswordRequiresLower),
        // Description = "Passwords must have at least one lowercase ('a'-'z')."
        Description = "パスワードには少なくとも 1 つの小文字  ('a'-'z') が必要です。"
    };

    public override IdentityError PasswordRequiresNonAlphanumeric() => new()
    {
        Code = nameof(PasswordRequiresNonAlphanumeric),
        // Description = "password must have at least one non alphanumeric character."
        Description = "パスワードには少なくとも 1 つの英数字以外の文字が含まれている必要があります。"
    };

    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) => new()
    {
        Code = nameof(PasswordRequiresUniqueChars),
        // Description = $"Passwords must use at least {uniqueChars} different characters."
        Description = $"パスワードには少なくとも {uniqueChars} 個の異なる文字を使用する必要があります。"
    };

    public override IdentityError PasswordRequiresUpper() => new()
    {
        Code = nameof(PasswordRequiresUpper),
        //Description = "Passwords must have at least one uppercase ('A'-'Z')."
        Description = "パスワードには少なくとも 1 つの大文字が必要です。 ('A'-'Z')."
    };

    public override IdentityError PasswordTooShort(int length) => new()
    {
        Code = nameof(PasswordTooShort),
        // Description = $"password must be at least {length} characters."
        Description = $"パスワードは少なくとも {length} 文字必要です。"
    };

    public override IdentityError RecoveryCodeRedemptionFailed() => new()
    {
        Code = nameof(RecoveryCodeRedemptionFailed),
        // Description = "Recovery code redemption failed."
        Description = "リカバリーコードの引き換えに失敗しました。"
    };

    public override IdentityError UserAlreadyHasPassword() => new()
    {
        Code = nameof(UserAlreadyHasPassword),
        // Description = "User already has a password set."
        Description = "ユーザーはすでにパスワードを設定しています。"
    };

    public override IdentityError UserAlreadyInRole(string role) => new()
    {
        Code = nameof(UserAlreadyInRole),
        // Description = $"User already in role '{role}'."
        Description = $"ユーザーはすでにロール「{role}」に属しています。"
    };

    public override IdentityError UserLockoutNotEnabled() => new()
    {
        Code = nameof(UserLockoutNotEnabled),
        // Description = "Lockout is not enabled for this user."
        Description = "このユーザーに対してロックアウトは有効になっていません。"
    };

    public override IdentityError UserNotInRole(string role) => new()
    {
        Code = nameof(UserNotInRole),
        // Description = $"User is not in role '{role}'."
        Description = $"ユーザーはロール「{role}」に属していません。"
    };

}