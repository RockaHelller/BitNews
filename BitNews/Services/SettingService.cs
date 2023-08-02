using BitNews.Data;
using BitNews.Services.Interfaces;

namespace BitNews.Services
{
	public class SettingService : ISettingService
	{
		private readonly AppDbContext _context;

		public SettingService(AppDbContext context)
		{
			_context = context;
		}
		public Dictionary<string, string> GetAll()
		{
			return _context.Settings.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
		}
	}
}
