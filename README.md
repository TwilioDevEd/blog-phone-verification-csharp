<a href="https://www.twilio.com">
  <img src="https://static0.twilio.com/marketing/bundles/marketing/img/logos/wordmark-red.svg" alt="Twilio" width="250" />
</a>

# Blog: Build a Simple Phone Verification System with Twilio, C#, SQL Server, and jQuery

## Local Development

This application demonstrates how to build a phone verification system in a hurry using
Twilio Voice, C#, SQL Server, and jQuery.

This project is built using [ASP.NET MVC](http://www.asp.net/mvc) Framework.

1. First clone this repository and `cd` into it.

   ```shell
   git clone git@github.com:TwilioDevEd/blog-phone-verification-csharp.git
   cd blog-phone-verification-csharp
   ```

1. Rename the sample configuration file and edit it to match your configuration.

   ```shell
   rename PhoneVerification.Web\Local.config.example PhoneVerification.Web\Local.config
   ```

   You can find your **Account SID** and **Auth Token** in your
   [Twilio Account](https://www.twilio.com/user/account/settings).

1. Build the solution.

1. Create database and run migrations.

   Make sure SQL Server is up and running.  
   In Visual Studio, open the following command in the [Package Manager
   Console](https://docs.nuget.org/consume/package-manager-console).

   ```shell
   Update-Database
   ```

1. Expose application to the wider internet. To [start using
   ngrok](https://www.twilio.com/blog/2015/09/6-awesome-reasons-to-use-ngrok-when-testing-webhooks.html)
   on our project you'll have to execute the following line in the command
   prompt.

   ```shell
   ngrok http 8080 -host-header="localhost:8080"
   ```

1. Update the base URL in the `Web.config` file as shown below.

   ```xml
   <!-- This will be the base URL for your call/twiml route on your server.-->
   <add key="BaseUrl" value="http://<subdomain>.ngrok.io" />
   ```

1. Run the application.

1. Check it out at [http://localhost:8080](http://localhost:8080).

## Meta

* No warranty expressed or implied. Software is as is. Diggity.
* [MIT License](http://www.opensource.org/licenses/mit-license.html)
* Lovingly crafted by Twilio Developer Education.
