import { useCallback, useState } from 'react';
import { useFocusEffect } from '@react-navigation/native';
import { StyleSheet, Text, View, Pressable, Linking } from 'react-native';
import { API_URL } from '../config';
import { obtenerGraduado, obtenerToken } from '../session';

export default function PerfilScreen({ navigation }) {
  const [perfil, setPerfil] = useState(null);
  const [error, setError] = useState(null);

  useFocusEffect(
    useCallback(() => {
      const cargar = async () => {
        try {
          const graduado = await obtenerGraduado();
          const token = await obtenerToken();

          const res = await fetch(`${API_URL}/graduados/${graduado.id}`, {
            headers: { Authorization: `Bearer ${token}` },
          });

          if (!res.ok) {
            setError('No se pudo cargar el perfil');
            return;
          }

          setPerfil(await res.json());
        } catch (err) {
          setError('No se pudo conectar con el servidor');
        }
      };

      cargar();
    }, [])
  );

  return (
    <View style={styles.container}>
      <Text style={styles.titulo}>Mi perfil</Text>

      {error && <Text style={styles.texto}>{error}</Text>}

      {perfil && (
        <View>
          <Text style={styles.etiqueta}>Nombre</Text>
          <Text style={styles.texto}>{perfil.nombre} {perfil.apellido}</Text>

          <Text style={styles.etiqueta}>Email</Text>
          <Text style={styles.texto}>{perfil.email}</Text>

          <Text style={styles.etiqueta}>Año de graduación</Text>
          <Text style={styles.texto}>{perfil.anioGraduacion}</Text>

          <Text style={styles.etiqueta}>Enlace a mi CV</Text>
          {perfil.cvUrl ? (
            <Pressable onPress={() => Linking.openURL(perfil.cvUrl)}>
              <Text style={styles.enlace}>{perfil.cvUrl}</Text>
            </Pressable>
          ) : (
            <Text style={styles.texto}>Todavía no cargaste tu CV</Text>
          )}
        </View>
      )}

      <Pressable onPress={() => navigation.navigate('EditarPerfil')}>
        <Text style={[styles.texto, styles.accion]}>Editar perfil</Text>
      </Pressable>

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
  titulo: { color: 'white', fontSize: 24, fontWeight: 'bold', marginBottom: 16 },
  etiqueta: { color: 'white', fontWeight: 'bold', marginTop: 12 },
  texto: { color: 'white', marginTop: 4 },
  enlace: { color: 'white', marginTop: 4, textDecorationLine: 'underline' },
  accion: { marginTop: 24 },
  volver: { color: 'white', textAlign: 'center', marginTop: 32 },
});