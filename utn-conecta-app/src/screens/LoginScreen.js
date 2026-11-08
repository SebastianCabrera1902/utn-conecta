import { useState } from 'react';
import { StyleSheet, Text, View, TextInput, Pressable, Alert } from 'react-native';
import { API_URL } from '../config';
import { guardarSesion, guardarSesionOrganizacion } from '../session';

export default function LoginScreen({ navigation }) {
  const [tipo, setTipo] = useState('graduado');
  const [email, setEmail] = useState('');
  const [contrasena, setContrasena] = useState('');

  const ingresar = async () => {
    try {
      const ruta = tipo === 'graduado' ? 'graduados' : 'organizaciones';
      const res = await fetch(`${API_URL}/${ruta}/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, contrasena }),
      });

      if (!res.ok) {
        Alert.alert('Error', 'Email o contraseña incorrectos');
        return;
      }

      const data = await res.json();
      if (tipo === 'graduado') {
        await guardarSesion(data.token, data.graduado);
      } else {
        await guardarSesionOrganizacion(data.token, data.organizacion);
      }
      navigation.navigate('Oportunidades');
    } catch (err) {
      Alert.alert('Error', 'No se pudo conectar con el servidor');
    }
  };

  return (
    <View style={styles.container}>
      <Text style={styles.titulo}>Iniciar sesión</Text>

      <View style={styles.selector}>
        <Pressable
          style={[styles.opcion, tipo === 'graduado' && styles.opcionActiva]}
          onPress={() => setTipo('graduado')}
        >
          <Text style={[styles.opcionTexto, tipo === 'graduado' && styles.opcionTextoActivo]}>
            Soy graduado
          </Text>
        </Pressable>
        <Pressable
          style={[styles.opcion, tipo === 'organizacion' && styles.opcionActiva]}
          onPress={() => setTipo('organizacion')}
        >
          <Text style={[styles.opcionTexto, tipo === 'organizacion' && styles.opcionTextoActivo]}>
            Soy organización
          </Text>
        </Pressable>
      </View>

      <TextInput
        style={styles.input}
        placeholder="Email"
        value={email}
        onChangeText={setEmail}
        autoCapitalize="none"
        keyboardType="email-address"
      />
      <TextInput
        style={styles.input}
        placeholder="Contraseña"
        value={contrasena}
        onChangeText={setContrasena}
        secureTextEntry
      />

      <Pressable style={styles.boton} onPress={ingresar}>
        <Text style={styles.botonTexto}>Ingresar</Text>
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
  titulo: {
    color: 'white',
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 16,
  },
  selector: {
    flexDirection: 'row',
    marginBottom: 16,
  },
  opcion: {
    flex: 1,
    borderWidth: 1,
    borderColor: 'white',
    borderRadius: 10,
    padding: 12,
    alignItems: 'center',
    marginRight: 8,
  },
  opcionActiva: { backgroundColor: 'white' },
  opcionTexto: { color: 'white', fontWeight: 'bold' },
  opcionTextoActivo: { color: '#015883' },
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