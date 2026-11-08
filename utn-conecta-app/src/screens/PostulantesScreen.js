import { useCallback, useState } from 'react';
import { useFocusEffect } from '@react-navigation/native';
import { StyleSheet, Text, View, FlatList, Pressable, Linking } from 'react-native';
import { API_URL } from '../config';
import { obtenerToken } from '../session';

export default function PostulantesScreen({ route, navigation }) {
  const { oportunidad } = route.params;
  const [postulantes, setPostulantes] = useState([]);
  const [mensaje, setMensaje] = useState('Cargando...');

  useFocusEffect(
    useCallback(() => {
      const cargar = async () => {
        try {
          const token = await obtenerToken();
          const res = await fetch(`${API_URL}/postulaciones/oportunidad/${oportunidad.id}`, {
            headers: { Authorization: `Bearer ${token}` },
          });

          if (!res.ok) {
            setMensaje('No se pudieron cargar los postulantes.');
            return;
          }

          const data = await res.json();
          setPostulantes(data);
          setMensaje(data.length === 0 ? 'Todavía nadie se postuló a esta oportunidad.' : '');
        } catch (err) {
          setMensaje('No se pudo conectar con el servidor');
        }
      };
      cargar();
    }, [oportunidad.id])
  );

  return (
    <View style={styles.container}>
      <Text style={styles.titulo}>Postulantes</Text>
      <Text style={styles.subtitulo}>{oportunidad.puesto}</Text>

      {mensaje !== '' && <Text style={styles.texto}>{mensaje}</Text>}

      <FlatList
        data={postulantes}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <View style={styles.card}>
            <Text style={styles.nombre}>{item.nombreGraduado}</Text>
            <Text style={styles.detalle}>{item.emailGraduado}</Text>
            <Text style={styles.detalle}>
              Se postuló el {new Date(item.fecha).toLocaleDateString('es-AR')}
            </Text>
            {item.cvUrlGraduado ? (
              <Pressable onPress={() => Linking.openURL(item.cvUrlGraduado)}>
                <Text style={styles.cv}>Ver CV</Text>
              </Pressable>
            ) : (
              <Text style={styles.detalle}>No cargó su CV</Text>
            )}
          </View>
        )}
      />

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
  subtitulo: { color: 'white', marginBottom: 16 },
  texto: { color: 'white', marginBottom: 12 },
  card: {
    backgroundColor: 'white',
    borderRadius: 10,
    padding: 14,
    marginBottom: 12,
  },
  nombre: { fontSize: 18, fontWeight: 'bold' },
  detalle: { color: '#555', marginTop: 4 },
  cv: { color: '#015883', marginTop: 8, fontWeight: 'bold' },
  volver: { color: 'white', textAlign: 'center', marginVertical: 16 },
});