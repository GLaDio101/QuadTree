namespace Project.Model.SimulationConfig
{
    public class SimulationConfigModel : ISimulationConfigModel
    {
        [PostConstruct]
        public void OnPostConstruct()
        {
            Config = new SimulationConfigVo();
        }

        public SimulationConfigVo Config { get; set; }
    }
}