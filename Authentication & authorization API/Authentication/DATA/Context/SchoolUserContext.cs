using Authentication.DATA.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.DATA;

public class SchoolUserContext : IdentityDbContext<USERS>
{
	public SchoolUserContext(DbContextOptions<SchoolUserContext> options):base(options)
	{

	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.Entity<USERS>().ToTable("SchoolUsers");
	}
}
