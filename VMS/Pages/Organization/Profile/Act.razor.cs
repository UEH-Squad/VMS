using VMS.Application.Interfaces;
using Microsoft.AspNetCore.Components;
using VMS.Application.ViewModels;
using System.Threading.Tasks;
using Blazored.Modal;
using System.Collections.Generic;
using Microsoft.JSInterop;
using Blazored.Modal.Services;

namespace VMS.Pages.Organization.Profile
{
    public partial class Act : ComponentBase
    {
        readonly int id = -1;

        [Parameter] public bool HaveControl { get; set; }
        [Parameter] public bool HaveFav { get; set; }
        [Parameter] public bool HaveDecor { get; set; }
        [Parameter] public bool HaveLogin { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public bool HaveHeart { get; set; } = false;
        [Parameter] public bool OverridesImg { get; set; } = false;
        [Parameter] public bool HaveBorder { get; set; } = true;
        [Parameter] public bool HaveLinkAll { get; set; } = false;
        [CascadingParameter] public IModalService Modal { get; set; }
        [Parameter] public bool Owner { get; set; }
        [Parameter] public List<ActivityViewModel> Datas { get; set; }
        [Parameter] public string QueryString { get; set; }
        [Parameter] public bool IsHomepage { get; set; } = true;

        [Inject]
        private IActivityService ActivityService { get; set; }
        [Inject]
        private IIdentityService IdentityService { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender && Datas.Count > 0)
            {
                await JS.InvokeVoidAsync("vms.OrganizeCarousel");
                await JS.InvokeVoidAsync("vms.AddOutsideClickMenuHandler", DotNetObjectReference.Create(this), nameof(HideMenuInterop));
            }
        }

        void ShowMenu(int id)
        {
            Datas.ForEach(a => a.IsMenu = a.Id == id && !a.IsMenu);
        }

        private async Task AnimateHeart(int id)
        {
            string userId = IdentityService.GetCurrentUserId();
            await ActivityService.UpdateActFavorAsync(id, userId);
            var act = Datas.Find(a => a.Id == id);
            act.IsFav = !act.IsFav;
        }

        [JSInvokable]
        public Task HideMenuInterop()
        {
            Datas.ForEach(a => a.IsMenu = false);
            return InvokeAsync(StateHasChanged);
        }

        private async Task ShowDeleteModal( int id)
        {
            var parameters = new ModalParameters();
            parameters.Add("ActId", id);
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true,
            };
            var result = await Modal.Show<DeleteConfirm>("", parameters, options).Result;

            if ((bool)result.Data)
            {
                var act = Datas.Find(a => a.Id == id);
                 Datas.Remove(act);
            }
        }

        private async Task ShowCloseModal(int id)
        {
            var act = Datas.Find(a => a.Id == id);
            
            var modalParams = new ModalParameters();
            modalParams.Add("IsClosed",act.IsClosed);

            var parameters = new ModalParameters();
            parameters.Add("ActId", id);
            parameters.Add("IsClosed", act.IsClosed);

            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true,
            };
            var result = await Modal.Show<CloseConfirm>("", parameters, options).Result;

            if ((bool)result.Data)
            {
                act.IsClosed = !act.IsClosed;
                Modal.Show<CloseSuccess>("", modalParams, options);
            }
        }

        private void ShowLoginRequire()
        {

            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true,
            };

            Modal.Show<LoginRequire>("", options);
        }

    }
}