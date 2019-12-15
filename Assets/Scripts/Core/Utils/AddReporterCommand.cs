using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace Core.Utils
{
  public class AddReporterCommand : EventCommand
  {
    [Inject(ContextKeys.CONTEXT_VIEW)]
    public GameObject RootObject { get; set; }

    public override void Execute()
    {
      Reporter reporter = RootObject.GetComponent<Reporter>();
      if (reporter == null)
      {
        RootObject.AddComponent<Reporter>();
        RootObject.AddComponent<ReporterMessageReceiver>();
      }

    }
  }
}