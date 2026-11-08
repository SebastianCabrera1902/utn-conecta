import { useEffect, useState } from 'react';
import { StyleSheet, Text, View, Pressable, Alert } from 'react-native';
import { API_URL } from '../config';
import { obtenerToken, obtenerTipo } from '../session';

export default function DetalleScreen({ route, navigation }) {
  const { oportunidad } = route.params;
  const [tipo, setTipo] = useState(null);

  useEffect(() => {
    obtenerTipo().then(setTipo);
  }, []);

  const postular = async () => {
    const token = await obtenerToken();

    if (!token) {
      Alert.alert('Iniciá sesión', 'Tenés que iniciar sesión para postularte');
      navigation.navigate('Login');
      return;
    }

    try {
      const res = await fetch(`${API_URL}/postulaciones`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({ oportunidadId: oportunidad.id }),
      });

      if (res.ok) {
        Alert.alert('Listo', 'Te postulaste correctamente');
      } else if (res.status === 400) {
        Alert.alert('Atención', 'Ya te postulaste a esta oportunidad');
      } else if (res.status === 401 || res.status === 403) {
        Alert.alert('Error', 'Tu sesión no es válida. Volvé a iniciar sesión');
      } else {
        Alert.alert('Error', 'No se pudo completar la postulación');
      }
    } catch (err) {
      Alert.alert('Error', 'No se pudo conectar con el servidor');
    }
  };

  return (
    <View style={styles.container}>
      <Text style={styles.titulo}>{oportunidad.puesto}</Text>
      <Text style={styles.org}>{oportunidad.nombreOrganizacion}</Text>

      <Text style={styles.etiqueta}>Descripción</Text>
      <Text style={styles.texto}>{oportunidad.descripcion}</Text>

      <Text style={styles.etiqueta}>Requisitos</Text>
      <Text style={styles.texto}>{oportunidad.requisitos}</Text>

      <Text style={styles.etiqueta}>Modalidad</Text>
      <Text style={styles.texto}>{oportunidad.tipo} · {oportunidad.modalidad}</Text>

      <Text style={styles.etiqueta}>Contacto</Text>
      <Text style={styles.texto}>{oportunidad.datosContacto}</Text>

      {tipo !== 'organizacion' && (
        <Pressable style={styles.boton} onPress={postular}>
          <Text style={styles.botonTexto}>Postularme</Text>
        </Pressable>
      )}

      <Pressable onPress={() => navigation.goBack()}>
        <Text style={styles.volver}>Volver</Text>
      </Pressable>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#015883',
    paddingTop: 60,
    paddingHorizontal: 16,
  },
  titulo: { color: 'white', fontSize: 24, fontWeight: 'bold' },
  org: { color: 'white', marginBottom: 16 },
  etiqueta: { color: 'white', fontWeight: 'bold', marginTop: 12 },
  texto: { color: 'white', marginTop: 4 },
  boton: {
    backgroundColor: 'white',
    borderRadius: 10,
    padding: 14,
    alignItems: 'center',
    marginTop: 24,
  },
  botonTexto: { color: '#015883', fontWeight: 'bold' },
  volver: { color: 'white', textAlign: 'center', marginTop: 16 },
});