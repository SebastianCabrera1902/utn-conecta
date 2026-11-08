import { useEffect, useState } from 'react';
import { StyleSheet, Text, View, TextInput, Pressable, Alert } from 'react-native';
import { API_URL } from '../config';
import { obtenerGraduado, obtenerToken, actualizarGraduado } from '../session';

export default function EditarPerfilScreen({ navigation }) {
  const [graduado, setGraduado] = useState(null);
  const [nombre, setNombre] = useState('');
  const [apellido, setApellido] = useState('');
  const [anio, setAnio] = useState('');
  const [cvUrl, setCvUrl] = useState('');

  useEffect(() => {
    obtenerGraduado().then((g) => {
      setGraduado(g);
      setNombre(g.nombre);
      setApellido(g.apellido);
      setAnio(String(g.anioGraduacion).slice(0, 4));
      setCvUrl(g.cvUrl ?? '');
    });
  }, []);

  const guardar = async () => {
    if (!nombre.trim() || !apellido.trim() || !anio.trim()) {
      Alert.alert('Atención', 'Completá todos los campos');
      return;
    }

    const anioNumero = parseInt(anio, 10);
    const anioActual = new Date().getFullYear();

    if (isNaN(anioNumero) || anioNumero < 1950 || anioNumero > anioActual) {
      Alert.alert('Atención', `El año de graduación tiene que estar entre 1950 y ${anioActual}`);
      return;
    }

    const cvLimpio = cvUrl.trim();

    if (cvLimpio && !/^https?:\/\//i.test(cvLimpio)) {
      Alert.alert('Atención', 'El enlace al CV tiene que empezar con http:// o https://');
      return;
    }

    try {
      const token = await obtenerToken();

      const res = await fetch(`${API_URL}/graduados/${graduado.id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          nombre,
          apellido,
          idCarrera: graduado.idCarrera,
          anioGraduacion: anioNumero,
          cvUrl: cvLimpio,
        }),
      });

      if (!res.ok) {
        Alert.alert('Error', 'No se pudo guardar el perfil');
        return;
      }

      await actualizarGraduado({
        ...graduado,
        nombre,
        apellido,
        anioGraduacion: anioNumero,
        cvUrl: cvLimpio || null,
      });

      Alert.alert('Listo', 'Perfil actualizado');
      navigation.goBack();
    } catch (err) {
      Alert.alert('Error', 'No se pudo conectar con el servidor');
    }
  };

  return (
    <View style={styles.container}>
      <Text style={styles.titulo}>Editar perfil</Text>

      <TextInput style={styles.input} placeholder="Nombre" value={nombre} onChangeText={setNombre} />
      <TextInput style={styles.input} placeholder="Apellido" value={apellido} onChangeText={setApellido} />
      <TextInput
        style={styles.input}
        placeholder="Año de graduación"
        value={anio}
        onChangeText={setAnio}
        keyboardType="numeric"
        maxLength={4}
      />
      <TextInput
        style={styles.input}
        placeholder="Enlace a tu CV (opcional)"
        value={cvUrl}
        onChangeText={setCvUrl}
        autoCapitalize="none"
        autoCorrect={false}
        keyboardType="url"
      />

      <Pressable style={styles.boton} onPress={guardar}>
        <Text style={styles.botonTexto}>Guardar</Text>
      </Pressable>

      <Pressable onPress={() => navigation.goBack()}>
        <Text style={styles.volver}>Cancelar</Text>
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
  input: {
    backgroundColor: 'white',
    borderRadius: 10,
    padding: 14,
    marginBottom: 12,
  },
  boton: {
    backgroundColor: 'white',
    borderRadius: 10,
    padding: 14,
    alignItems: 'center',
    marginBottom: 16,
  },
  botonTexto: { color: '#015883', fontWeight: 'bold' },
  volver: { color: 'white', textAlign: 'center' },
});