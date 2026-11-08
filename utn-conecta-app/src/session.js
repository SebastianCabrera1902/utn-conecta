import * as SecureStore from 'expo-secure-store';

export async function guardarSesion(token, graduado) {
  await SecureStore.setItemAsync('token', token);
  await SecureStore.setItemAsync('tipo', 'graduado');
  await SecureStore.setItemAsync('graduado', JSON.stringify(graduado));
  await SecureStore.deleteItemAsync('organizacion');
}

export async function guardarSesionOrganizacion(token, organizacion) {
  await SecureStore.setItemAsync('token', token);
  await SecureStore.setItemAsync('tipo', 'organizacion');
  await SecureStore.setItemAsync('organizacion', JSON.stringify(organizacion));
  await SecureStore.deleteItemAsync('graduado');
}

export async function obtenerToken() {
  return await SecureStore.getItemAsync('token');
}

export async function obtenerTipo() {
  return await SecureStore.getItemAsync('tipo');
}

export async function obtenerGraduado() {
  const texto = await SecureStore.getItemAsync('graduado');
  return texto ? JSON.parse(texto) : null;
}

export async function obtenerOrganizacion() {
  const texto = await SecureStore.getItemAsync('organizacion');
  return texto ? JSON.parse(texto) : null;
}

export async function cerrarSesion() {
  await SecureStore.deleteItemAsync('token');
  await SecureStore.deleteItemAsync('tipo');
  await SecureStore.deleteItemAsync('graduado');
  await SecureStore.deleteItemAsync('organizacion');
}

export async function actualizarGraduado(graduado) {
  await SecureStore.setItemAsync('graduado', JSON.stringify(graduado));
}