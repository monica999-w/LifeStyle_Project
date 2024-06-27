import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useAuth } from '../../../components/provider/AuthProvider';
import UpdateProfile from '../update/UpdateProfile';
import { environment } from '../../../environments/environment'; 
import { useNotification } from '../../../components/provider/NotificationContext';
import {
    Avatar,
    Box,
    Container,
    Dialog,
    Typography,
    IconButton,
    List,
    ListItem,
    ListItemIcon,
    ListItemText,
    Paper
} from '@mui/material';
import { FaEnvelope, FaPhone, FaRulerVertical, FaWeight, FaBirthdayCake, FaTransgender, FaEdit } from 'react-icons/fa';
import { Chart as ChartJS, CategoryScale, LinearScale, LineElement, PointElement, Title, Tooltip, Legend } from 'chart.js';
import { Line } from 'react-chartjs-2';

ChartJS.register(CategoryScale, LinearScale, LineElement, PointElement, Title, Tooltip, Legend);

interface UserProfile {
    profileId: number;
    email: string;
    phoneNumber: string;
    height: number;
    weight: number;
    birthDate: string;
    gender: number;
    photoUrl: string;
}

interface WeightHistory {
    date: string;
    weight: number;
}

const UserProfile: React.FC = () => {
    const { token } = useAuth();
    const { notify } = useNotification();
    const [profile, setProfile] = useState<UserProfile | null>(null);
    const [weightHistory, setWeightHistory] = useState<WeightHistory[]>([]);
    const [error, setError] = useState('');
    const [isModalOpen, setIsModalOpen] = useState(false);
    const apiUrl = `${environment.apiUrl}Auth/profile`;

    const fetchProfile = async () => {
        try {
            const response = await axios.get<UserProfile>(apiUrl, {
                headers: { Authorization: `Bearer ${token}` }
            });
            setProfile(response.data);
            notify('Profile fetched successfully.', 'success');
        } catch (error) {
            setError('Failed to fetch profile.');
            notify('Failed to fetch profile.', 'error');
        }
    };

    const fetchWeightHistory = async () => {
        try {
            const response = await axios.get<WeightHistory[]>(`${environment.apiUrl}Auth/profile/weightHistory`, {
                headers: { Authorization: `Bearer ${token}` }
            });
            setWeightHistory(response.data);
            notify('Weight history fetched successfully.', 'success');
        } catch (error) {
            setError('Failed to fetch weight history.');
            notify('Failed to fetch weight history.', 'error');
        }
    };

    useEffect(() => {
        fetchProfile();
        fetchWeightHistory();
    }, [token]);

    const calculateBMI = (weight: number, height: number) => {
        if (weight > 0 && height > 0) {
            const heightInMeters = height / 100;
            return (weight / (heightInMeters * heightInMeters)).toFixed(2);
        }
        return null;
    };

    const bmi = profile ? calculateBMI(profile.weight, profile.height) : null;

    const bmiDescription = () => {
        if (bmi) {
            const bmiValue = parseFloat(bmi);
            if (bmiValue < 18.5) {
                return "You are underweight. It's important to follow a balanced diet that includes a variety of foods, including proteins, carbohydrates, and healthy fats. Consider consulting with a nutritionist to create a meal plan that helps you gain weight in a healthy manner. Additionally, incorporating strength training exercises can help you build muscle mass.";
            } else if (bmiValue >= 18.5 && bmiValue < 24.9) {
                return "You have a normal weight. Great job! Maintain your healthy lifestyle by continuing to eat a balanced diet rich in fruits, vegetables, whole grains, and lean proteins. Regular physical activity, such as walking, jogging, or yoga, will help you stay fit. Remember to stay hydrated and get enough sleep for overall well-being.";
            } else if (bmiValue >= 25 && bmiValue < 29.9) {
                return "You are overweight. To reach a healthier weight, consider making some lifestyle changes. Start by incorporating more physical activities into your daily routine, such as brisk walking, cycling, or swimming. Focus on eating a balanced diet with plenty of vegetables, fruits, and lean proteins while reducing your intake of sugary and high-fat foods. It may also be beneficial to consult with a healthcare provider or a dietitian for personalized advice.";
            } else {
                return "You are obese. It's important to address this with the help of healthcare professionals. Begin by scheduling a consultation with your doctor to discuss your health and any potential underlying conditions. A nutritionist can help you develop a sustainable and healthy eating plan. Regular physical activity is crucial; start with low-impact exercises and gradually increase intensity. Behavioral therapy and support groups can also provide additional help and motivation.";
            }
        }
        return "";
    };

    if (!profile) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div className="error">{error}</div>;
    }

    const weightData = {
        labels: weightHistory.map(entry => new Date(entry.date).toLocaleDateString()),
        datasets: [
            {
                label: 'Weight',
                data: weightHistory.map(entry => entry.weight),
                borderColor: 'rgba(75,192,192,1)',
                backgroundColor: 'rgba(75,192,192,0.2)',
                borderWidth: 2,
                tension: 0.1
            },
        ],
    };

    const weightOptions = {
        scales: {
            y: {
                beginAtZero: true
            }
        }
    };

    return (
        <Container className="profile-container">
            <Box display="flex" justifyContent="space-between" mb={3}>
                <Typography variant="h4"> Profile</Typography>
                <IconButton color="primary" onClick={() => setIsModalOpen(true)}>
                    <FaEdit />
                </IconButton>
            </Box>
            <Box display="flex" justifyContent="space-between">
                <Box flex={1} mr={2}>
                    <Box display="flex" flexDirection="column" alignItems="center">
                        {profile.photoUrl && (
                            <Avatar
                                src={`https://localhost:44353${profile.photoUrl}`}
                                alt="Profile"
                                sx={{ width: 100, height: 100 }}
                                onError={(e: React.SyntheticEvent<HTMLImageElement, Event>) => {
                                    e.currentTarget.src = 'https://www.allnumis.ro/media/users/profiles/default/50.png';
                                }}
                            />
                        )}
                    </Box>
                    <List>
                        <ListItem>
                            <ListItemIcon><FaEnvelope /></ListItemIcon>
                            <ListItemText primary="Email" secondary={profile.email} />
                        </ListItem>
                        <ListItem>
                            <ListItemIcon><FaPhone /></ListItemIcon>
                            <ListItemText primary="Phone" secondary={profile.phoneNumber} />
                        </ListItem>
                        <ListItem>
                            <ListItemIcon><FaRulerVertical /></ListItemIcon>
                            <ListItemText primary="Height" secondary={`${profile.height} cm`} />
                        </ListItem>
                        <ListItem>
                            <ListItemIcon><FaWeight /></ListItemIcon>
                            <ListItemText primary="Weight" secondary={`${profile.weight} kg`} />
                        </ListItem>
                        <ListItem>
                            <ListItemIcon><FaBirthdayCake /></ListItemIcon>
                            <ListItemText primary="Birth Date" secondary={new Date(profile.birthDate).toISOString().split('T')[0]} />
                        </ListItem>
                        <ListItem>
                            <ListItemIcon><FaTransgender /></ListItemIcon>
                            <ListItemText primary="Gender" secondary={profile.gender === 0 ? 'Male' : 'Female'} />
                        </ListItem>
                    </List>
                </Box>
                <Box flex={1} sx={{ mt: 18 }}>
                    <Box width="100%">
                        <Typography variant="h6">Weight History</Typography>
                        <Line data={weightData} options={weightOptions} />
                    </Box>
                </Box>
            </Box>
            <Box mt={3} component={Paper} p={2}>
                <Typography variant="h4">BMI Result</Typography>
                {bmi ? (
                    <Typography variant="h6" sx={{ mt: 2 }}>
                        Your BMI is <strong>{bmi}</strong>. {bmiDescription()}
                    </Typography>
                ) : (
                    <Typography variant="h6" sx={{ mt: 2 }}>Please update your profile to see your BMI result.</Typography>
                )}
            </Box>
            <Dialog open={isModalOpen} onClose={() => setIsModalOpen(false)}>
                <Box>
                    <UpdateProfile closeModal={() => setIsModalOpen(false)} refreshProfile={() => window.location.reload()} />
                </Box>
            </Dialog>
        </Container>
    );
};

export default UserProfile;
