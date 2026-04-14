export function outputLog(obj) {
  console.log(typeof obj, obj)
}

export function listenForStorageChanges(dotNetRef, key) {
  const handler = (e) => {
    if (e.key === key) {
      dotNetRef.invokeMethodAsync("OnStorageChanged")
    }
  }
  window.addEventListener("storage", handler)
  return { dispose: () => window.removeEventListener("storage", handler) }
}
