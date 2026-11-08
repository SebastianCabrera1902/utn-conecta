import { useCallback, useState } from 'react';
import { useFocusEffect } from '@react-navigation/native';
import { API_URL } from '../config';
import { obtenerGraduado, obtenerOrganizacion, obtenerTipo, cerrarSesion } from '../session';
import { StyleSheet, Text, View, FlatList, Pressable } from 'react-native';

export default function OportunidadesScreen({ navigation }) {
  const [oportunidades, setOportunidades] = useState([]);
  const [error, setError] = useState(null);
  const [tipo, setTipo] = useState(null);
  const [usuario, setUsuario] = useState(null);

  useFocusEffect(
    useCallback(() => {
      const cargarSesion = async () => {
        const graduado = await obtenerGraduado();
        const organizacion = await obtenerOrganizacion();
        const tipoGuardado = await obtenerTipo();

        if (tipoGuardado === 'organizacion' && organizacion) {
          setTipo('organizacion');
          setUsuario(organizacion);
        } else if (graduado) {
          setTipo('graduado');
          setUsuario(graduado);
        } else {
          setTipo(null);
          setUsuario(null);
        }
      };
      cargarSesion();

      fetch(`${API_URL}/oportunidades`)
        .then(res => res.json())
        .then(data => setOportunidades(data))
        .catch(err => setError(err.message));
    }, [])
  );

  const salir = async () => {
    await cerrarSesion();
    setTipo(null);
    setUsuario(null);
  };

  return (
    <View style={styles.container}>
      <Text style={styles.titulo}>UTN CONECTA</Text>

      {usuario ? (
        <View style={styles.sesionBloque}>
          <View style={styles.fila}>
            <Text style={styles.texto}>Hola, {usuario.nombre}</Text>
            <Pressable onPress={salir}>
              <Text style={styles.texto}>Cerrar sesión</Text>
            </Pressable>
          </View>

          {tipo === 'graduado' && (
            <View style={styles.fila}>
              <Pressable onPress={() => navigation.navigate('Perfil')}>
                <Text style={styles.texto}>Mi perfil</Text>
              </Pressable>
              <Pressable onPress={() => navigation.navigate('MisPostulaciones')}>
                <Text style={styles.texto}>Mis postulaciones</Text>
              </Pressable>
            </View>
          )}

        {tipo === 'organizacion' && (
        <View style={styles.fila}>
          <Pressable onPress={() => navigation.navigate('PublicarOportunidad')}>
            <Text style={styles.texto}>Publicar oportunidad</Text>
          </Pressable>
          <Pressable onPress={() => navigation.navigate('MisOportunidades')}>
            <Text style={styles.texto}>Mis oportunidades</Text>
        </Pressable>
        </View>
)}
        </View>
      ) : (
        <Pressable onPress={() => navigation.navigate('Login')}>
          <Text style={styles.texto}>Iniciar sesión</Text>
        </Pressable>
      )}

      {error && <Text style={styles.texto}>Error: {error}</Text>}

      <FlatList
        data={oportunidades}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <Pressable onPress={() => navigation.navigate('Detalle', { oportunidad: item })}>
            <View style={styles.card}>
              <Text style={styles.puesto}>{item.puesto}</Text>
              <Text style={styles.org}>{item.nombreOrganizacion}</Text>
              <Text style={styles.detalle}>{item.tipo} · {item.modalidad}</Text>
            </View>
          </Pressable>
        )}
      />
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
  texto: {
    color: 'white',
  },
  sesionBloque: {
    marginBottom: 12,
  },
  fila: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 8,
  },
  titulo: {
    color: 'white',
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 16,
  },
  card: {
    backgroundColor: 'white',
    borderRadius: 10,
    padding: 14,
    marginBottom: 12,
  },
  puesto: { fontSize: 18, fontWeight: 'bold' },
  org: { color: '#015883', marginTop: 4 },
  detalle: { color: '#555', marginTop: 4 },
});