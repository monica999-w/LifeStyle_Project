import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useAuth } from '../../../components/provider/AuthProvider';
import UpdateProfile from '../update/UpdateProfile';
import { environment } from '../../../environments/environment'; 
import { useNotification } from '../../../components/provider/NotificationContext';
import {
    Avatar,
    Box,
    Button,
    Container,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Typography,
    IconButton,
    List,
    ListItem,
    ListItemIcon,
    ListItemText,
    Alert,
    Paper
} from '@mui/material';
import { FaEnvelope, FaPhone, FaRulerVertical, FaWeight, FaBirthdayCake, FaTransgender, FaEdit } from 'react-icons/fa';
import { Chart as ChartJS, CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend } from 'chart.js';
import { Bar } from 'react-chartjs-2';

ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

interface UserProfile {
    profileId: number;
    email: string;
    phoneNumber: string;
    height: number;
    weight: number;
    birthDate: string;
    gender: string;
    photoUrl: string;
}

const UserProfile: React.FC = () => {
    const { token } = useAuth();
    const { notify } = useNotification();
    const [profile, setProfile] = useState<UserProfile | null>(null);
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

    useEffect(() => {
        fetchProfile();
    }, [token]);

    if (!profile) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div className="error">{error}</div>;
    }

    const calculateBMI = (weight: number, height: number) => {
        if (weight > 0 && height > 0) {
            const heightInMeters = height / 100;
            return (weight / (heightInMeters * heightInMeters)).toFixed(2);
        }
        return null;
    };

    const bmi = calculateBMI(profile.weight, profile.height);

    const bmiData = {
        labels: ['BMI'],
        datasets: [
            {
                label: 'BMI',
                data: bmi ? [parseFloat(bmi)] : [],
                backgroundColor: 'rgba(75,192,192,1)',
                borderColor: 'rgba(75,192,192,1)',
                borderWidth: 1,
            },
        ],
    };

    const bmiOptions = {
        scales: {
            y: {
                beginAtZero: true,
                max: 40
            }
        }
    };

    const bmiDescription = () => {
        if (bmi) {
            const bmiValue = parseFloat(bmi);
            if (bmiValue < 18.5) {
                return "You are underweight. It is important to follow a balanced diet that includes a variety of foods, including proteins, carbohydrates, and healthy fats. Consider consulting with a nutritionist to create a meal plan that helps you gain weight in a healthy manner. Additionally, incorporating strength training exercises can help you build muscle mass.";
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

    return (
        <Container className="profile-container">
            <Box display="flex" justifyContent="space-between" mb={3}>
                <Typography variant="h4">User Profile</Typography>
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
                            <ListItemText primary="Birth Date" secondary={new Date(profile.birthDate).toLocaleDateString()} />
                        </ListItem>
                        <ListItem>
                            <ListItemIcon><FaTransgender /></ListItemIcon>
                            <ListItemText primary="Gender" secondary={profile.gender} />
                        </ListItem>
                    </List>
                </Box>
                <Box flex={1} sx={{ mt: 18 }}>
                    {bmi ? (
                        <Box width="100%">
                            <Typography variant="h6">Your BMI</Typography>
                            <Bar data={bmiData} options={bmiOptions} />
                        </Box>
                    ) : (
                        <Alert severity="warning">Please update your profile to calculate BMI.</Alert>
                    )}
                </Box>
            </Box>
            <Box mt={3} component={Paper} p={2}>
                <Typography variant="h4">BMI Result</Typography>
                {bmi ? (
                    <Typography variant="h6" sx={{ mt: 2 }}>
                        Your BMI is <strong>{bmi}</strong>. {bmiDescription()}
                    </Typography>
                ) : (
                    <Alert severity="warning">Please update your profile to see your BMI result.</Alert>
                )}
            </Box>
            <Dialog open={isModalOpen} onClose={() => setIsModalOpen(false)}>
                <DialogTitle>Update Profile</DialogTitle>
                <DialogContent>
                    <UpdateProfile closeModal={() => setIsModalOpen(false)} refreshProfile={fetchProfile} />
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setIsModalOpen(false)} color="primary">
                        Close
                    </Button>
                </DialogActions>
            </Dialog>
        </Container>
    );
};

export default UserProfile;
