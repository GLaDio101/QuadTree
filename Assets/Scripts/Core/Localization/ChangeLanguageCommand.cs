using strange.extensions.command.impl;
using Service.Localization;

namespace Core.Localization
{
    public class ChangeLanguageCommand : EventCommand
    {
        [Inject]
        public ILocalizationService localizationService { get; set; }

        public override void Execute()
        {
            localizationService.NextLanguage();
        }
    }
}