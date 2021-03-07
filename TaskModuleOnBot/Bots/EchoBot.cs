// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.11.1

using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveCards;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema.Teams;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TaskModuleOnBot.Bots
{
    public class EchoBot : TeamsActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var replyText = turnContext.Activity.Text;
            if (replyText.ToLower() == "hi" || replyText.ToLower() == "hello")
            {
                await turnContext.SendActivityAsync(MessageFactory.Attachment(FirstCard()), cancellationToken);
            }
        }
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }

        protected override async Task<TaskModuleResponse> OnTeamsTaskModuleFetchAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(taskModuleRequest);
            await turnContext.SendActivityAsync(json);
            var obj = JObject.Parse(json);
            var name = (string)obj["data"]["name"];
            return new TaskModuleResponse
            {
                Task = new TaskModuleContinueResponse
                {
                    Value = new TaskModuleTaskInfo()
                    {
                        Card = TaskModuleCard(name),
                        Height = 400,
                        Width = 400,
                        Title = "Adaptive Card ",
                    },
                }
            };
        }
        protected override async Task<TaskModuleResponse> OnTeamsTaskModuleSubmitAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            var reply = MessageFactory.Text("Feedback Completed: " + JsonConvert.SerializeObject(taskModuleRequest));
            await turnContext.SendActivityAsync(reply);


            return new TaskModuleResponse
            {
                Task = new TaskModuleMessageResponse()
                {
                    Value = "Thanks!",
                },
            };
        }

        private Attachment FirstCard()
        {
            AdaptiveCard card = new AdaptiveCard("1.0")
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveTextBlock
                    {
                        Text="Login",
                        Size=AdaptiveTextSize.Large,
                        Weight=AdaptiveTextWeight.Bolder,
                        HorizontalAlignment=AdaptiveHorizontalAlignment.Center,
                    },
                    new AdaptiveColumnSet
                    {
                        Columns=new List<AdaptiveColumn>()
                        {
                            new AdaptiveColumn
                            {
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Text="Enter Name : ",
                                        Size=AdaptiveTextSize.Large,

                                    }
                                },
                                Width=AdaptiveColumnWidth.Auto

                            },
                            new AdaptiveColumn
                            {
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextInput
                                    {
                                        Placeholder="Name",
                                        Id="name"
                                    }

                                }
                            }
                        }
                    },
                    new AdaptiveColumnSet
                    {
                        Columns=new List<AdaptiveColumn>()
                        {
                            new AdaptiveColumn
                            {
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Text="Enter Password : ",
                                        Size=AdaptiveTextSize.Large,

                                    }
                                },
                                Width=AdaptiveColumnWidth.Auto

                            },
                            new AdaptiveColumn
                            {
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextInput
                                    {
                                        Placeholder="Password",
                                        Id="pwd"
                                    }

                                }
                            }
                        }
                    },
                    new AdaptiveActionSet
                    {
                        Actions=new List<AdaptiveAction>()
                        {
                            new AdaptiveSubmitAction
                            {
                                Title = "Login",
                                Style = "positive",
                                Type=AdaptiveSubmitAction.TypeName,
                                Data= new Dictionary<string, object>()
                                {
                                    {
                                        "msteams",new Dictionary<string,string>()
                                        {
                                            {
                                                "type","task/fetch"
                                            },
                                            {
                                                "value","{\"Id\":\"name\"}"
                                            }
                                        }
                                    },
                                    {
                                    "data","login"
                                    }
                                }
                            },

                        }
                    }
                }
            };
            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return attachment;
        }
        private Attachment TaskModuleCard(string name)
        {
            AdaptiveCard card = new AdaptiveCard("1.0")
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveTextBlock
                    {
                        Text="thanks "+name+ " for login"
                    },
                    new AdaptiveColumnSet
                    {
                        Columns=new List<AdaptiveColumn>()
                        {
                            new AdaptiveColumn
                            {
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Text="Please Give Feedback : ",
                                        Size=AdaptiveTextSize.Small,

                                    }
                                },
                                Width=AdaptiveColumnWidth.Auto

                            },
                            new AdaptiveColumn
                            {
                                Items=new List<AdaptiveElement>()
                                {
                                    new AdaptiveTextInput
                                    {
                                        Placeholder="feedback",
                                        Id="fb"
                                    }

                                }
                            }
                        }
                    },

                    new AdaptiveActionSet
                    {
                        Actions=new List<AdaptiveAction>()
                        {
                            new AdaptiveSubmitAction
                            {
                                Title = "submit",
                                Style = "positive",
                                Type=AdaptiveSubmitAction.TypeName,
                                Data= new Dictionary<string, object>()
                                {
                                    {
                                        "msteams",new Dictionary<string,string>()
                                        {
                                            {
                                                "type","task/submit"
                                            },
                                            {
                                                "value","{\"Id\":\"fb\"}"
                                            }
                                        }
                                    },
                                    {
                                    "data","submit"
                                    }
                                }
                            },

                        }
                    }
                }
            };
            Attachment at = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            return at;
        }

    }

}
