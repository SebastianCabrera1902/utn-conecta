import { useCallback, useState } from 'react';
import { useFocusEffect } from '@react-navigation/native';
import { StyleSheet, Text, View, FlatList, Pressable } from 'react-native';
import { API_URL } from '../config';
import { obtenerToken } from '../session';

export default function MisOportunidadesScreen({ navigation }) {
  const [oportunidades, setOportunidades] = useState([]);
  const [mensaje, setMensaje] = useState('Cargando...');

  useFocusEffect(
    useCallback(() => {
      const cargar = async () => {
        try {
          const token = await obtenerToken();
          const res = await fetch(`${API_URL}/oportunidades/mias`, {
            headers: { Authorization: `Bearer ${token}` },
          });

          if (!res.ok) {
            setMensaje('No se pudieron cargar tus oportunidades. Volvé a iniciar sesión.');
            return;
          }

          const data = await res.json();
          setOportunidades(data);
          setMensaje(data.length === 0 ? 'Todavía no publicaste ninguna oportunidad.' : '');
        } catch (err) {
          setMensaje('No se pudo conectar con el servidor');
        }
      };
      cargar();
    }, [])
  );

  return (
    <View style={styles.container}>
      <Text style={styles.titulo}>Mis oportunidades</Text>

      {mensaje !== '' && <Text style={styles.texto}>{mensaje}</Text>}

      <FlatList
        data={oportunidades}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <Pressable onPress={() => navigation.navigate('Postulantes', { oportunidad: item })}>
            <View style={styles.card}>
              <Text style={styles.puesto}>{item.puesto}</Text>
              <Text style={styles.detalle}>{item.tipo} · {item.modalidad}</Text>
              <Text style={styles.ver}>Ver postulantes</Text>
            </View>
          </Pressable>
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
  texto: { color: 'white', marginBottom: 12 },
  card: {
    backgroundColor: 'white',
    borderRadius: 10,
    padding: 14,
    marginBottom: 12,
  },
  puesto: { fontSize: 18, fontWeight: 'bold' },
  detalle: { color: '#555', marginTop: 4 },
  ver: { color: '#015883', marginTop: 8, fontWeight: 'bold' },
  volver: { color: 'white', textAlign: 'center', marginVertical: 16 },
});