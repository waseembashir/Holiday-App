﻿public void ConfigureAuth(IAppBuilder app)
{
// Configure the db context and user manager to use a single instance per request
    app.CreatePerOwinContext(ApplicationDbContext.Create);
app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

// Enable the application to use a cookie to store information for the signed in user
// and to use a cookie to temporarily store information about a user logging in with a third party login provider
// Configure the sign in cookie
app.UseCookieAuthentication(new CookieAuthenticationOptions
{
    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
    LoginPath = new PathString("/Account/Login"),
    Provider = new CookieAuthenticationProvider
{
        OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
validateInterval: TimeSpan.FromMinutes(30),
regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
}
});
    
app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

// Uncomment the following lines to enable logging in with third party login providers
//app.UseMicrosoftAccountAuthentication(
//    clientId: "434225719515-gb7gp0pfpc24rvp1rq4co9b2v9834no4.apps.googleusercontent.com",
//    clientSecret: "AiaNqRbUE6PGNmtfybNKRuCX");

//app.UseTwitterAuthentication(
//   consumerKey: "",
//   consumerSecret: "");

//app.UseFacebookAuthentication(
//   appId: "",
//   appSecret: "");

app.UseGoogleAuthentication(
    clientId: "434225719515-gb7gp0pfpc24rvp1rq4co9b2v9834no4.apps.googleusercontent.com",
    clientSecret: "AiaNqRbUE6PGNmtfybNKRuCX");
}