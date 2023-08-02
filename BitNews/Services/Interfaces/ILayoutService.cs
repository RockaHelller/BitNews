using BitNews.ViewModels;

namespace BitNews.Services.Interfaces
{
	public interface ILayoutService
	{
		Task<LayoutVM> GetAllDatas();
	}
}
