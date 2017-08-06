using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.BLL.Pooling.Models.Abstract;
using App.BLL.VehiclesAbroad.Models.Filters;
using App.Classes.DAL.Reservations.Abstract;
using System.Web.UI.WebControls;

namespace Mars.Pooling.Models.Abstract {
    public abstract class LabelModel {

        public Label TextLabel { get; set; }
        public string Text {
            get {
                if(TextLabel == null) throw new NullReferenceException("The TextLabel is null and hasn't been instantiated to a new System.Web.UI.WebControls.Label, in the LabelModel abstract class.");
                return TextLabel.Text;
            }
            set {
                if(TextLabel == null) throw new NullReferenceException("The TextLabel is null and hasn't been instantiated to a new System.Web.UI.WebControls.Label, in the LabelModel abstract class.");
                TextLabel.Text = value;
            }
        }
        public Label ErrorLabel { get; set; }
        public string ErrorText {
            get {
                if(ErrorLabel == null) throw new NullReferenceException("The ErrorLabel is null and hasn't been instantiated to a new System.Web.UI.WebControls.Label, in the LabelModel abstract class.");
                return ErrorLabel.Text;
            }
            set {
                if(ErrorLabel == null) throw new NullReferenceException("The ErrorLabel is null and hasn't been instantiated to a new System.Web.UI.WebControls.Label, in the LabelModel abstract class.");
                
                ErrorLabel.Text = value;
            }
        }
        public virtual void Update() {
            throw new NotImplementedException("The virtual method Update has no implementation in the LabelModel abstract class, please override this method when inheriting this class.");
        }
    }
}