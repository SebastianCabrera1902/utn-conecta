import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { StatusBar } from 'expo-status-bar';
import OportunidadesScreen from './src/screens/OportunidadesScreen';
import LoginScreen from './src/screens/LoginScreen';
import DetalleScreen from './src/screens/DetalleScreen';
import PerfilScreen from './src/screens/PerfilScreen';
import MisPostulacionesScreen from './src/screens/MisPostulacionesScreen';
import EditarPerfilScreen from './src/screens/EditarPerfilScreen';
import PublicarOportunidadScreen from './src/screens/PublicarOportunidadScreen';
import MisOportunidadesScreen from './src/screens/MisOportunidadesScreen';
import PostulantesScreen from './src/screens/PostulantesScreen';

const Stack = createNativeStackNavigator();

export default function App() {
  return (
    <NavigationContainer>
      <StatusBar style="light" />
      <Stack.Navigator screenOptions={{ headerShown: false }}>
       
        <Stack.Screen name="Oportunidades" component={OportunidadesScreen} />
        <Stack.Screen name="Login" component={LoginScreen} />
        <Stack.Screen name="Detalle" component={DetalleScreen}/>
        <Stack.Screen name="Perfil" component={PerfilScreen}/>
        <Stack.Screen name="MisPostulaciones" component={MisPostulacionesScreen}/>
        <Stack.Screen name="EditarPerfil" component={EditarPerfilScreen} />
        <Stack.Screen name="PublicarOportunidad" component={PublicarOportunidadScreen}/>
        <Stack.Screen name="MisOportunidades" component={MisOportunidadesScreen} />
        <Stack.Screen name="Postulantes" component={PostulantesScreen} />


      </Stack.Navigator>
    </NavigationContainer>
  );
}