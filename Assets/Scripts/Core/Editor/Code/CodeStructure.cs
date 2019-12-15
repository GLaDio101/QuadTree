namespace Core.Editor.Code
{
  public class CodeStructure
  {
    private static void AddContext(string name)
    {
      Template context = new Template(TemplateType.Context);
      context.Name(name).Save();

      Template bootstrap = new Template(TemplateType.Bootstrap);
      bootstrap.Name(name).Save();
    }
  }
}