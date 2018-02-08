using UnityEngine;

namespace ProtoBox.Console.Commands
{
    public class GraphicsCommands : ConsoleCommand
    {
        private SubCommand[] m_commands;

        public override string Name
        { get { return "graphics"; } }

        public override SubCommand[] Commands
        { get { return m_commands; } }

        private void SetAA(string[] args)
        {
            QualitySettings.antiAliasing = ParseInt(args[2]);
        }

        private void SetVSync(string[] args)
        {
            QualitySettings.vSyncCount = ParseInt(args[2]);
        }

        private void SetShadowQuality(string[] args)
        {
            QualitySettings.shadows = StringToEnum<ShadowQuality>(args[2]);
        }

        private void SetAnsio(string[] args)
        {
            QualitySettings.anisotropicFiltering = StringToEnum<AnisotropicFiltering>(args[2]);//StrToAniso(args[2]);
            
        }

        private void SetQualityLevel(string[] args)
        {
            QualitySettings.SetQualityLevel(ParseInt(args[2]), true);
            Debug.Log("Quality set to: \"" + QualitySettings.names[QualitySettings.GetQualityLevel()] + "\"");
        }

        private void IncreaseQualityLevel(string[] args)
        {
            QualitySettings.IncreaseLevel(true);
            Debug.Log("Quality set to: \"" + QualitySettings.names[QualitySettings.GetQualityLevel()] + "\"");
        }

        private void DecreaseQualityLevel(string[] args)
        {
            QualitySettings.DecreaseLevel(true);
            Debug.Log("Quality set to: \"" + QualitySettings.names[QualitySettings.GetQualityLevel()] + "\"");
        }

        public GraphicsCommands()
        {
            m_commands = new SubCommand[]
            {
                new SubCommand("setaa",                 new string[] { "aa" },                             new Param[] { new Param(typeof(int),           "AntiAliasing level") }, SetAA),
                new SubCommand("setvsync",              new string[] { "vsync", "v" },                     new Param[] { new Param(typeof(int),           "vsync count") }, SetVSync),
                new SubCommand("setshadowquality",      new string[] { "shadowquality", "shadow", "shq" }, new Param[] { new Param(typeof(ShadowQuality), "ShadowQuality") }, SetShadowQuality),
                new SubCommand("setqualitylevel",       new string[] { "qualitylevel", "quality", "q" },   new Param[] { new Param(typeof(int),           "Quality Level") }, SetShadowQuality),
                new SubCommand("increasequalitylevel",  new string[] { "increasequality", "iq" },          null , IncreaseQualityLevel),
                new SubCommand("decreasequalitylevel",  new string[] { "decreasequality", "dq" },          null , DecreaseQualityLevel)
            };
        }
    }
}
