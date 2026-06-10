using Microsoft.Data.SqlClient;

namespace Cinema.AppHost;

internal static class DatabaseResourceBuilderExtensions
{
    public static IResourceBuilder<SqlServerDatabaseResource> WithClearMoviesCommand(this IResourceBuilder<SqlServerDatabaseResource> builder)
    {
        var commandOptions = new CommandOptions
        {
            IconName = "DatabaseWarning",
            IconVariant = IconVariant.Filled
        };

        builder.WithCommand(
            name: "clear-movies",
            displayName: "Clear Movies",
            executeCommand: context => OnClearMoviesCommandAsync(builder, context),
            commandOptions: commandOptions);

        return builder;
    }

    private static async Task<ExecuteCommandResult> OnClearMoviesCommandAsync(IResourceBuilder<SqlServerDatabaseResource> builder, ExecuteCommandContext context)
    {
        var connectionString = await builder.Resource.ConnectionStringExpression.GetValueAsync(context.CancellationToken);

        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(context.CancellationToken);
        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Movies";
        await command.ExecuteNonQueryAsync(context.CancellationToken);

        return CommandResults.Success("Movies cleared successfully!");
    }
}