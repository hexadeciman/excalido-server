using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderIndexToBoard2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DO $$ 
                DECLARE column_exists BOOLEAN;
                BEGIN 
                    SELECT EXISTS (
                        SELECT 1 FROM information_schema.columns 
                        WHERE table_name = 'Boards' AND column_name = 'OrderIndex'
                    ) INTO column_exists;

                    IF column_exists THEN
                        -- Update OrderIndex with ordered values per OwnerId
                        UPDATE public.""Boards""
                        SET ""OrderIndex"" = subquery.NewOrderIndex
                        FROM (
                            SELECT 
                                ""Id"", 
                                DENSE_RANK() OVER (PARTITION BY ""OwnerId"" ORDER BY ""Id"") - 1 AS NewOrderIndex
                            FROM public.""Boards""
                        ) AS subquery
                        WHERE public.""Boards"".""Id"" = subquery.""Id"";
                    END IF;
                END $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
