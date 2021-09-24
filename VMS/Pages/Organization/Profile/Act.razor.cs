using VMS.Application.Interfaces;
using Microsoft.AspNetCore.Components;
using VMS.Application.ViewModels;
using Blazored.Modal.Services;
using System.Threading.Tasks;
using Blazored.Modal;
using System.Collections.Generic;
using Microsoft.JSInterop;

namespace VMS.Pages.Organization.Profile
{
    public partial class Act : ComponentBase
    {
        int pendingId = -1;
        bool isDeleteConfirm;
        bool isCloseConfirm;
        bool isDeleteSuccess;
        bool isCloseSuccess;

        [Parameter] public bool HaveControl { get; set; } = false;
        [Parameter] public bool HaveDecor { get; set; } = true;
        [Parameter] public string Title { get; set; }
        [Parameter] public bool Owner { get; set; }
        [Parameter] public string UserId { get; set; }
        [Parameter] public List<ActivityViewModel> Datas { get; set; }
        [CascadingParameter] public IModalService Modal { get; set; }
        [Inject]
        private IActivityService ActivityService { get; set; }

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
            pendingId = id;
            Datas.ForEach(a => a.IsMenu = a.Id == pendingId ? !a.IsMenu : false);
        }

        [JSInvokable]
        public Task HideMenuInterop()
        {
            Datas.ForEach(a => a.IsMenu = false);
            return InvokeAsync(StateHasChanged);
        }

        void DeleteConfirm()
        {
            isDeleteConfirm = !isDeleteConfirm;
            Datas.ForEach(a => a.IsMenu = false);
        }

        void CloseConfirm()
        {
            isCloseConfirm = !isCloseConfirm;
            Datas.ForEach(a => a.IsMenu = false);
        }

        private async Task DeleteSuccess()
        {
            if (isDeleteConfirm == true)
            {
                isDeleteConfirm = false;
            }
            isDeleteSuccess = !isDeleteSuccess;
            await ActivityService.UpdateStatusActAsync(pendingId, "deleted");
            var act = Datas.Find(a => a.Id == pendingId);
            act.IsDeleted = true;
        }

        private async Task CloseSuccess()
        {
            if (isCloseConfirm == true)
            {
                isCloseConfirm = false;
            }

            isCloseSuccess = !isCloseSuccess;
            await ActivityService.UpdateStatusActAsync(pendingId, "closed");
            var act = Datas.Find(a => a.Id == pendingId);
            act.IsClosed = true;
        }

        private void ShowModal()
        {
            var options = new ModalOptions()
            {
                HideCloseButton = true,
                DisableBackgroundCancel = true,
                UseCustomLayout = true,
            };

            Modal.Show<DeleteConfirm>("", options);
        }
    }
}