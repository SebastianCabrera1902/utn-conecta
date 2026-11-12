import { useCallback, useState } from 'react';
import { useFocusEffect } from '@react-navigation/native';
import { API_URL } from '../config';
import { obtenerGraduado, obtenerOrganizacion, obtenerTipo, cerrarSesion } from '../session';
import { StyleSheet, Text, View, FlatList, Pressable } from 'react-native';

const OPCIONES_TIPO = [
  { etiqueta: 'Todos', valor: null },
  { etiqueta: 'Trabajo', valor: 'trabajo' },
  { etiqueta: 'Pasantía', valor: 'pasantía' },
];

const OPCIONES_MODALIDAD = [
  { etiqueta: 'Todas', valor: null },
  { etiqueta: 'Presencial', valor: 'presencial' },
  { etiqueta: 'Híbrido', valor: 'híbrido' },
  { etiqueta: 'Remoto', valor: 'remoto' },
];

export default function OportunidadesScreen({ navigation }) {
  const [oportunidades, setOportunidades] = useState([]);
  const [error, setError] = useState(null);
  const [tipo, setTipo] = useState(null);
  const [filtroTipo, setFiltroTipo] = useState(null);
  const [filtroModalidad, setFiltroModalidad] = useState(null);
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

      const params = [];
      if (filtroTipo) params.push(`tipo=${encodeURIComponent(filtroTipo)}`);
      if (filtroModalidad) params.push(`modalidad=${encodeURIComponent(filtroModalidad)}`);
      const url = `${API_URL}/oportunidades${params.length ? '?' + params.join('&') : ''}`;

      fetch(url)
        .then(res => res.json())
        .then(data => setOportunidades(data))
        .catch(err => setError(err.message));
    }, [filtroTipo, filtroModalidad])
  );

  const salir = async () => {
    await cerrarSesion();
    setTipo(null);
    setUsuario(null);
  };

  const renderFiltros = (opciones, valorActual, alElegir) => (
    <View style={styles.filtros}>
      {opciones.map(op => (
        <Pressable
          key={op.etiqueta}
          onPress={() => alElegir(op.valor)}
          style={[styles.chip, valorActual === op.valor && styles.chipActivo]}
        >
          <Text style={valorActual === op.valor ? styles.chipTextoActivo : styles.chipTexto}>
            {op.etiqueta}
          </Text>
        </Pressable>
      ))}
    </View>
  );

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

      {renderFiltros(OPCIONES_TIPO, filtroTipo, setFiltroTipo)}
      {renderFiltros(OPCIONES_MODALIDAD, filtroModalidad, setFiltroModalidad)}

      <FlatList
        data={oportunidades}
        keyExtractor={(item) => item.id.toString()}
        ListEmptyComponent={<Text style={styles.texto}>No hay oportunidades con esos filtros.</Text>}
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
  filtros: { flexDirection: 'row', flexWrap: 'wrap', marginBottom: 4 },
  chip: {
    borderWidth: 1,
    borderColor: 'white',
    borderRadius: 16,
    paddingVertical: 6,
    paddingHorizontal: 14,
    marginRight: 8,
    marginBottom: 8,
  },
  chipActivo: { backgroundColor: 'white' },
  chipTexto: { color: 'white' },
  chipTextoActivo: { color: '#015883', fontWeight: 'bold' },
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