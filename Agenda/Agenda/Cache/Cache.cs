using Serilog;

namespace Agenda.Cache;

/// <summary>
///     Implementación de caché basada en el algoritmo LRU (Least Recently Used).
///     Cuando la capacidad se agota, elimina el elemento que lleva más tiempo sin ser utilizado.
/// </summary>
public class Cache<TKey, TValue> : ICache<TKey, TValue>
    where TKey : notnull {
    private readonly int _capacity;
    private readonly Dictionary<TKey, TValue> _data = new();
    private readonly ILogger _logger = Log.ForContext<Cache<TKey, TValue>>();
    private readonly LinkedList<TKey> _usageOrder = new();

    public Cache(int capacity) {
        if (capacity <= 0)
            throw new ArgumentException("La capacidad debe ser mayor que 0.", nameof(capacity));
        _capacity = capacity;
    }

    /// <inheritdoc cref="ICache{TKey,TValue}.Add" />
    public void Add(TKey key, TValue value) {
        _logger.Debug("[CACHE-ADD] Intentando añadir clave: {Key}", key);

        if (_data.TryGetValue(key, out var existingValue)) {
            _logger.Debug("[CACHE-ADD] Clave {Key} ya existe. Actualizando valor: {Old} -> {New}",
                key, existingValue, value);
            _data[key] = value;
            RefreshUsage(key);
            return;
        }

        _logger.Debug("[CACHE-ADD] Clave {Key} es nueva. Capacidad actual: {Used}/{Total}",
            key, _data.Count, _capacity);

        if (_data.Count >= _capacity) {
            var oldestKey = _usageOrder.First!.Value;
            var oldestValue = _data[oldestKey];
            _logger.Debug("[CACHE-EVICT] Cache llena. Desalojando elemento más antiguo: {Key} = {Value}",
                oldestKey, oldestValue);
            _usageOrder.RemoveFirst();
            _data.Remove(oldestKey);
        }

        _data.Add(key, value);
        _usageOrder.AddLast(key);
        _logger.Debug("[CACHE-ADD] Elemento añadido. Nueva lista de uso: {Order}",
            string.Join(" -> ", _usageOrder));
    }

    /// <inheritdoc cref="ICache{TKey,TValue}.Get" />
    public TValue? Get(TKey key) {
        _logger.Debug("[CACHE-GET] Buscando clave: {Key}", key);

        if (!_data.TryGetValue(key, out var value)) {
            _logger.Debug("[CACHE-GET] Clave {Key} NO encontrada en cache", key);
            return default;
        }

        _logger.Debug("[CACHE-GET] Clave {Key} encontrada con valor: {Value}.",
            key, value);
        RefreshUsage(key);
        _logger.Debug("[CACHE-GET] Lista tras búsqueda: {Order}",
            string.Join(" -> ", _usageOrder));

        return value;
    }

    /// <inheritdoc cref="ICache{TKey,TValue}.Remove" />
    public bool Remove(TKey key) {
        _logger.Debug("[CACHE-REMOVE] Intentando eliminar clave: {Key}", key);

        if (!_data.Remove(key)) {
            _logger.Debug("[CACHE-REMOVE] Clave {Key} no encontrada", key);
            return false;
        }

        _usageOrder.Remove(key);
        _logger.Debug("[CACHE-REMOVE] Clave {Key} eliminada correctamente", key);
        return true;
    }

    /// <summary>
    ///     Mueve una clave existente a la última posición de la lista de uso.
    /// </summary>
    /// <param name="key">La clave del elemento que acaba de ser utilizado.</param>
    private void RefreshUsage(TKey key) {
        _logger.Verbose("[CACHE-REFRESH] Moviendo clave {Key} al final de la lista", key);
        _usageOrder.Remove(key);
        _usageOrder.AddLast(key);
    }
}