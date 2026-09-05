namespace PosID.Application.Exceptions;

public sealed class NotFoundException(string resource, int id) : Exception($"No se encontró {resource} con identificador {id}.");
public sealed class ConflictException(string message) : Exception(message);
