using Modal = Blazored.Modal.ModalOptions;

namespace VMS.Common
{
    public static class BlazoredModalOptions
    {
        public static Modal GetModalOptions()
        {
            return new()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true
            };
        }
    }
}