import { useState } from 'react';
import { StyleSheet, Text, View, TextInput, Pressable, Alert, ScrollView } from 'react-native';
import { API_URL } from '../config';
import { obtenerToken } from '../session';

const TIPOS = ['trabajo', 'pasantía'];
const MODALIDADES = ['remoto', 'híbrido', 'presencial'];

function Opciones({ valores, elegido, onElegir }) {
  return (
    <View style={styles.opciones}>
      {valores.map((v) => (
        <Pressable
          key={v}
          style={[styles.opcion, elegido === v && styles.opcionActiva]}
          onPress={() => onElegir(v)}
        >
          <Text style={[styles.opcionTexto, elegido === v && styles.opcionTextoActivo]}>{v}</Text>
        </Pressable>
      ))}
    </View>
  );
}

export default function PublicarOportunidadScreen({ navigation }) {
  const [puesto, setPuesto] = useState('');
  const [descripcion, setDescripcion] = useState('');
  const [requisitos, setRequisitos] = useState('');
  const [areaProfesional, setAreaProfesional] = useState('');
  const [tipo, setTipo] = useState('trabajo');
  const [modalidad, setModalidad] = useState('remoto');
  const [datosContacto, setDatosContacto] = useState('');

  const publicar = async () => {
    const token = await obtenerToken();
    if (!token) {
      Alert.alert('Error', 'Tu sesión no es válida. Volvé a iniciar sesión');
      return;
    }

    try {
      const res = await fetch(`${API_URL}/oportunidades`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          puesto,
          descripcion,
          requisitos,
          areaProfesional,
          tipo,
          modalidad,
          datosContacto,
        }),
      });

      if (res.ok) {
        Alert.alert('Listo', 'La oportunidad se publicó correctamente');
        navigation.goBack();
      } else if (res.status === 400) {
        const mensaje = await res.text();
        Alert.alert('Revisá los datos', mensaje);
      } else if (res.status === 401 || res.status === 403) {
        Alert.alert('Error', 'Tu sesión no es válida. Volvé a iniciar sesión');
      } else {
        Alert.alert('Error', 'No se pudo publicar la oportunidad');
      }
    } catch (err) {
      Alert.alert('Error', 'No se pudo conectar con el servidor');
    }
  };

  return (
    <ScrollView
      style={styles.container}
      contentContainerStyle={styles.contenido}
      keyboardShouldPersistTaps="handled"
    >
      <Text style={styles.titulo}>Publicar oportunidad</Text>

      <Text style={styles.etiqueta}>Puesto</Text>
      <TextInput style={styles.input} value={puesto} onChangeText={setPuesto} />

      <Text style={styles.etiqueta}>Descripción</Text>
      <TextInput
        style={[styles.input, styles.inputGrande]}
        value={descripcion}
        onChangeText={setDescripcion}
        multiline
      />

      <Text style={styles.etiqueta}>Requisitos</Text>
      <TextInput
        style={[styles.input, styles.inputGrande]}
        value={requisitos}
        onChangeText={setRequisitos}
        multiline
      />

      <Text style={styles.etiqueta}>Área profesional</Text>
      <TextInput
        style={styles.input}
        value={areaProfesional}
        onChangeText={setAreaProfesional}
        placeholder="Ej: sistemas, mecatrónica, higiene y seguridad"
      />

      <Text style={styles.etiqueta}>Tipo</Text>
      <Opciones valores={TIPOS} elegido={tipo} onElegir={setTipo} />

      <Text style={styles.etiqueta}>Modalidad</Text>
      <Opciones valores={MODALIDADES} elegido={modalidad} onElegir={setModalidad} />

      <Text style={styles.etiqueta}>Datos de contacto</Text>
      <TextInput
        style={styles.input}
        value={datosContacto}
        onChangeText={setDatosContacto}
        autoCapitalize="none"
        placeholder="Email o teléfono"
      />

      <Pressable style={styles.boton} onPress={publicar}>
        <Text style={styles.botonTexto}>Publicar</Text>
      </Pressable>

      <Pressable onPress={() => navigation.goBack()}>
        <Text style={styles.volver}>Volver</Text>
      </Pressable>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#015883' },
  contenido: { paddingTop: 60, paddingHorizontal: 16, paddingBottom: 40 },
  titulo: { color: 'white', fontSize: 24, fontWeight: 'bold', marginBottom: 8 },
  etiqueta: { color: 'white', fontWeight: 'bold', marginTop: 12, marginBottom: 4 },
  input: { backgroundColor: 'white', borderRadius: 10, padding: 14 },
  inputGrande: { minHeight: 80, textAlignVertical: 'top' },
  opciones: { flexDirection: 'row', flexWrap: 'wrap' },
  opcion: {
    borderWidth: 1,
    borderColor: 'white',
    borderRadius: 10,
    paddingVertical: 10,
    paddingHorizontal: 14,
    marginRight: 8,
    marginBottom: 8,
  },
  opcionActiva: { backgroundColor: 'white' },
  opcionTexto: { color: 'white', fontWeight: 'bold' },
  opcionTextoActivo: { color: '#015883' },
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