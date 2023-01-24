using Microsoft.EntityFrameworkCore;
using MiniValidation;
using TesteTecnico.Data;
using TesteTecnico.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TesteTecnicoDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/usuario", async (
        TesteTecnicoDB context) =>
        await context.Usuarios.ToListAsync())
        .WithName("GetUsuario")
        .WithTags("Usuario");

app.MapGet("/usuario/{id}", async (
    Guid id,
    TesteTecnicoDB context) =>
    await context.Usuarios.FindAsync(id)
          is Usuario usuario
              ? Results.Ok(usuario)
              : Results.NotFound())
    .Produces<Usuario>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("GetUsuarioPorId")
    .WithTags("Usuario");

app.MapPost("/usuario", async (
    TesteTecnicoDB context,
    Usuario usuario) =>
{
    if (!MiniValidator.TryValidate(usuario, out var errors))
        return Results.ValidationProblem(errors);

    context.Usuarios.Add(usuario);
    var result = await context.SaveChangesAsync();

    return result > 0
        ? Results.CreatedAtRoute("GetUsuarioPorId", new { id = usuario.Id }, usuario)
        : Results.BadRequest("Houve um problema ao salvar o registro");

}).ProducesValidationProblem()
.Produces<Usuario>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest)
.WithName("PostUsuario")
.WithTags("Usuario");


app.MapPut("/usuario/{id}", async (
    Guid id,
    TesteTecnicoDB context,
    Usuario usuario) =>
{
    var usuarioBanco = await context.Usuarios.AsNoTracking<Usuario>().FirstOrDefaultAsync(f => f.Id == id);
    if (usuarioBanco == null) return Results.NotFound();

    if (!MiniValidator.TryValidate(usuario, out var errors))
        return Results.ValidationProblem(errors);

    context.Usuarios.Update(usuario);
    var result = await context.SaveChangesAsync();

    return result > 0
        ? Results.NoContent()
        : Results.BadRequest("Houve um problema ao salvar o registro");

}).ProducesValidationProblem()
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status400BadRequest)
.WithName("PutUsuario")
.WithTags("Usuario");

app.MapDelete("/usuario/{id}", async (
    Guid id,
    TesteTecnicoDB context) =>
{
    var usuario = await context.Usuarios.FindAsync(id);
    if (usuario == null) return Results.NotFound();

    context.Usuarios.Remove(usuario);
    var result = await context.SaveChangesAsync();

    return result > 0
        ? Results.NoContent()
        : Results.BadRequest("Houve um problema ao salvar o registro");

}).Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound)
.WithName("DeleteUsuario")
.WithTags("Usuario");

app.Run();
