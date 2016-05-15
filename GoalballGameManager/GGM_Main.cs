using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoalballGameManager
{
    /// <summary>
    ///     Starts the GGM Form
    /// </summary>
    static class GGM_Main
    {
        /// <summary>
        ///     The main entry point for the application
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new GGM_Form());
        }
    }
}
