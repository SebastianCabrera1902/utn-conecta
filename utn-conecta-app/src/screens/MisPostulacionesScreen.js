import { useEffect, useState } from 'react';
import { StyleSheet, Text, View, FlatList, Pressable } from 'react-native';
import { API_URL } from '../config';
import { obtenerToken } from '../session';

export default function MisPostulacionesScreen({ navigation }) {
  const [postulaciones, setPostulaciones] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    const cargar = async () => {
      try {
        const token = await obtenerToken();

        const res = await fetch(`${API_URL}/postulaciones/mias`, {
          headers: { Authorization: `Bearer ${token}` },
        });

        if (!res.ok) {
          setError('No se pudieron cargar tus postulaciones');
          return;
        }

        setPostulaciones(await res.json());
      } catch (err) {
        setError('No se pudo conectar con el servidor');
      }
    };

    cargar();
  }, []);

  return (
    <View style={styles.container}>
      <Text style={styles.titulo}>Mis postulaciones</Text>

      {error && <Text style={styles.texto}>{error}</Text>}

      {!error && postulaciones.length === 0 && (
        <Text style={styles.texto}>Todavía no te postulaste a ninguna oportunidad.</Text>
      )}

      <FlatList
        data={postulaciones}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <View style={styles.card}>
            <Text style={styles.puesto}>{item.puestoOportunidad}</Text>
            <Text style={styles.detalle}>
              {new Date(item.fecha).toLocaleDateString('es-AR')} · {item.estado}
            </Text>
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
  titulo: { color: 'white', fontSize: 24, fontWeight: 'bold', marginBottom: 16 },
  texto: { color: 'white' },
  card: {
    backgroundColor: 'white',
    borderRadius: 10,
    padding: 14,
    marginBottom: 12,
  },
  puesto: { fontSize: 18, fontWeight: 'bold' },
  detalle: { color: '#555', marginTop: 4 },
  volver: { color: 'white', textAlign: 'center', marginVertical: 16 },
});