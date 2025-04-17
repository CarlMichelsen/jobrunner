using Microsoft.EntityFrameworkCore;

namespace Database;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options);