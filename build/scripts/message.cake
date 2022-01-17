///////////////////////////////////////////////////////////////////////////////
// MESSAGE
///////////////////////////////////////////////////////////////////////////////

Task("Slack")
    .WithCriteria(() => !isPullRequest)
    .Does(() =>
{
    // Resolve the webHook Url.
    var webHookUrl = EnvironmentVariable("SLACK_WEBHOOK_URL");

    if (string.IsNullOrEmpty(webHookUrl))
    {
        throw new InvalidOperationException("Could not resolve Slack webHook Url.");
    }



    // Attachments
    var text = "";
    var packages = false;

    for (int index = 0; index < projectNames.Count; index++)
    {
        if (projectNames[index] != appName)
        {
            text += projectNames[index] + "." + version + ".nupkg";
            packages = true;
        }

        if (index < (projectNames.Count - 1))
        {
            text += "\n";
        }
    }



    // Title
    var title = "";

    if (packages)
    {
        title = "Published <https://www.nuget.org/packages/" + appName + "/|" + appName + " v" + version + ">";
    }
    else
    {
        title = "Published " + appName + " v" + version;
    }



    // Post Message
    SlackChatMessageResult result;

    SlackChatMessageSettings settings = new SlackChatMessageSettings()
    {
        IncomingWebHookUrl = webHookUrl,
        UserName = "Cake",
        IconUrl = new System.Uri("https://cdn.jsdelivr.net/gh/cake-build/graphics/png/cake-small.png")
    };

    if (packages)
    {
        IList<SlackChatMessageAttachment> attachments = new List<SlackChatMessageAttachment>();

        attachments.Add(new SlackChatMessageAttachment()
        {
            Color = "good",
            Text = text
        });

        result = Slack.Chat.PostMessage("#code", title, attachments, settings);
    }
    else
    {
        result = Slack.Chat.PostMessage("#code", title, settings);
    }



    // Check Result
    if (result.Ok)
    {
        // Posted
        Information("Message was succcessfully sent to Slack.");
    }
    else
    {
        // Error
        Error("Failed to send message to Slack: {0}", result.Error);
    }
});