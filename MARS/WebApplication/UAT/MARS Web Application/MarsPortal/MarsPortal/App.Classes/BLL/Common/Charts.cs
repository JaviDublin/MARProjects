
using App.BLL;

namespace Mars.App.Classes.BLL.Common
{
    public class Charts
    {
        public static System.Drawing.Color GetColourForColumn(string columnName)
        {

            System.Drawing.Color colour;


            switch (columnName)
            {
                case "TOTAL FLEET":
                    colour = System.Drawing.Color.Black;
                    break;
                case "CARSALES":
                case "SALES":
                    colour = System.Drawing.Color.FromArgb(128, 0, 128);
                    break;
                case "CARHOLD H":
                    colour = System.Drawing.Color.FromArgb(255, 255, 0);
                    break;
                case "CARHOLD L":
                    colour = System.Drawing.Color.FromArgb(255, 255, 153);
                    break;
                case "CU":
                    colour = System.Drawing.Color.FromArgb(255, 153, 204);
                    break;
                case "HA":
                    colour = System.Drawing.Color.FromArgb(255, 153, 0);
                    break;
                case "HL":
                    colour = System.Drawing.Color.FromArgb(153, 51, 0);
                    break;
                case "LL":
                    colour = System.Drawing.Color.FromArgb(153, 51, 0);
                    break;
                case "NC":
                    colour = System.Drawing.Color.FromArgb(255, 204, 0);
                    break;
                case "PL":
                    colour = System.Drawing.Color.FromArgb(128, 128, 0);
                    break;
                case "TC":
                    colour = System.Drawing.Color.FromArgb(255, 128, 128);
                    break;
                case "SV":
                    colour = System.Drawing.Color.FromArgb(255, 0, 255);
                    break;
                case "WS (NONRAC)":
                    colour = System.Drawing.Color.FromArgb(128, 0, 128);
                    break;
                case "WS":
                    colour = System.Drawing.Color.FromArgb(128, 0, 128);
                    break;
                case "OPERATIONAL FLEET":
                    colour = System.Drawing.Color.FromArgb(0, 255, 0);
                    break;
                case "BD":
                    colour = System.Drawing.Color.FromArgb(0, 0, 255);
                    break;
                case "MM":
                    colour = System.Drawing.Color.FromArgb(153, 153, 255);
                    break;
                case "TW":
                    colour = System.Drawing.Color.FromArgb(0, 0, 255);
                    break;
                case "TB":
                    colour = System.Drawing.Color.FromArgb(51, 204, 204);
                    break;
                case "WS (RAC)":
                    colour = System.Drawing.Color.FromArgb(153, 204, 255);
                    break;
                case "FS":
                    colour = System.Drawing.Color.FromArgb(0, 0, 255);
                    break;
                case "RL":
                    colour = System.Drawing.Color.FromArgb(0, 0, 128);
                    break;
                case "RP":
                    colour = System.Drawing.Color.FromArgb(51, 51, 153);
                    break;
                case "TN":
                    colour = System.Drawing.Color.OrangeRed;
                    break;
                case "AVAILABLE FLEET":
                case "MNT":
                    colour = System.Drawing.Color.FromArgb(0, 170, 0);
                    break;
                case "RT":
                case "Idle":
                    colour = System.Drawing.Color.FromArgb(255, 0, 0);
                    break;
                case "SU":
                    colour = System.Drawing.Color.FromArgb(192, 192, 192);
                    break;
                case "GOLD":

                    colour = System.Drawing.Color.FromArgb(255, 204, 0);
                    break;
                case "PREDELIVERY":
                    colour = System.Drawing.Color.FromArgb(204, 255, 204);
                    break;
                case "OVERDUE":
                    colour = System.Drawing.Color.FromArgb(150, 0, 0);
                    break;
                case "ON RENT":
                case "RNT":
                    colour = System.Drawing.Color.DarkGreen;
                    break;
                case "UNAVAILABLE NON-OPS FLEET":
                case "UTILIZATION":
                    colour = System.Drawing.Color.Navy;
                    break;
                case "UNAVAILABLE OPERATIONAL FLEET":
                case "UTILIZATION OVERDUE":
                    colour = System.Drawing.Color.PowderBlue;
                    break;
                case "PREDELIVERY & GOLD":

                    colour = System.Drawing.Color.LightGreen;
                    break;
                case "IDLE":
                    colour = System.Drawing.Color.FromArgb(255, 0, 0);
                    break;
                case "SHOP":
                    colour = System.Drawing.Color.SeaGreen;
                    break;
                case "DEFLEET":
                    colour = System.Drawing.Color.Sienna;
                    break;
                default:
                    colour = System.Drawing.Color.Gray;

                    break;
            }


            return colour;

        }
    }
}