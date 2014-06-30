<Query Kind="Program">
  <NuGetReference>Microsoft.Bcl.Async</NuGetReference>
  <NuGetReference>Microsoft.Net.Http</NuGetReference>
  <NuGetReference>WindowsAzure.ServiceBus</NuGetReference>
  <Namespace>Microsoft.ServiceBus.Notifications</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	var client = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://derpnotificationhub-ns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=LSR+QjdmW2dlnRvQXqhjbr2Fv4tzLEgVT9Qfhyem4dI=",
	"derpnotificationhub");
	
	client.Dump();
	
	Register(client).Wait();
	
	SendNotification(client).Wait();
}

async Task Register(NotificationHubClient client)
{
	var allRegistrations = await client.GetAllRegistrationsAsync(10);
	allRegistrations.Dump();


	var registrationId = "APA91bH_VMmiRcqjEM7RzhnCTskm7YtY9J4S5xYd_Cfoa8jqq4FVgTWpWJECdsqNOJUD1n06E9K4Okm7y0YxRYTiw-_fY-8CnV4G9ktd7HKdeUB8MjYvHn-CoQlZ_1xG4Gy3YgwmPi1wZUsLSdWEyP3E4Nbb17QPqePiuvI5O0Wq4Vov62vwDvM";
	var description = await client.CreateGcmNativeRegistrationAsync(registrationId);
	description.Dump();
	
	allRegistrations = await client.GetAllRegistrationsAsync(10);
	allRegistrations.Dump();
}

async Task SendNotification(NotificationHubClient client)
{
	var outcome = await client.SendGcmNativeNotificationAsync("{ \"data\" : {\"msg\":\"Hello from Azure!\"}}");
	outcome.Dump();
}

// Define other methods and classes here
