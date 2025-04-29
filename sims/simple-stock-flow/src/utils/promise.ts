
/**
 * Usage:
 * ```
 * const deferred = create_deferred_promise<string>()
 * deferred.promise.then(result => console.log(result))
 * deferred.resolve("abc") // Logs: "abc"
 * ```
 */
export function create_deferred_promise<T>()
{
    let resolve: (value: T | PromiseLike<T>) => void
    let reject: (reason?: any) => void

    const promise = new Promise<T>((res, rej) =>
    {
        resolve = res
        reject = rej
    })

    return {
        promise,
        resolve: resolve!,
        reject: reject!,
    }
}
