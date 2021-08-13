using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Domain.Models;

namespace VMS.Pages.Activities
{
	public partial class AreasPopup
	{
		private List<Area> areas;
		private List<Area> selectedAreas;

		[Inject]
		private IAreaService AreaService { get; set; }

		protected async override Task OnInitializedAsync()
		{
			areas = await AreaService.GetAllAreasAsync();
			selectedAreas = new List<Area>();
		}

		private void OnClickArea(Area area)
		{
			if (!selectedAreas.Contains(area))
			{
				selectedAreas.Add(area);
			}
			else
			{
				selectedAreas.Remove(area);
			}
		}

		private void OnClickSave()
		{
			ModalResult.Ok(selectedAreas);
		}
	}
}
