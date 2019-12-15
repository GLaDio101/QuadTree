using System.Collections.Generic;
using System.Linq;
using Core.Manager.Bundle;
using Core.Promise;
using strange.extensions.command.impl;

namespace Project.Controller.Bootstrap
{
  public class LoadBundlesCommand : EventCommand
  {
    private readonly List<string> BundleList = new List<string>
    {
      "common",
      "shaders",
//      "fonts"
    };

    [Inject]
    public IBundleModel bundleModel { get; set; }

    public override void Execute()
    {
      Retain();

      Promise<BundleLoadData>
        .All(BundleList.Select(bundleInfo =>
          bundleModel.LoadBundle(bundleInfo, true)))
        .Done(links => { Release(); });
    }
  }
}