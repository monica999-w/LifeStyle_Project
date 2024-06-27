import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';
import { Box, Typography, IconButton } from '@mui/material';
import { environment } from '../../../environments/environment';
import { useAuth } from '../../../components/provider/AuthProvider';
import { useNotification } from '../../../components/provider/NotificationContext';
import FavoriteBorderIcon from '@mui/icons-material/FavoriteBorder';
import FavoriteIcon from '@mui/icons-material/Favorite';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { FaDumbbell, FaRunning } from 'react-icons/fa';
import routes from '../../../components/route/routes';

interface Exercise {
    id: number;
    name: string;
    durationInMinutes: number;
    description: string;
    videoLink: string;
    type: number;
    equipment: number;
    majorMuscle: number;
}

const getTypeString = (type: number) => {
    const types = [
        'Yoga',
        'Cardio',
        'Pilates',
        'CrossFit',
        'Aerobics',
    ];
    return types[type] || 'Unknown';
};

const getEquipmentString = (equipment: number) => {
    const equipments = [
        'BodyWeight',
        'Band',
        'Barbell',
        'Dumbbell',
        'Kettlebell',
        'Machine',
        'MedicineBall',
    ];
    return equipments[equipment] || 'Unknown';
};

const getMajorMuscleString = (majorMuscle: number) => {
    const muscles = [
        'Arms',
        'Back',
        'Core',
        'Legs',
        'Shoulders',
        'FullBody'
    ];
    return muscles[majorMuscle] || 'Unknown';
};

const ExerciseDetails: React.FC = () => {
    const { token, email } = useAuth();
    const { notify } = useNotification();
    const { exerciseId } = useParams<{ exerciseId: string }>();
    const navigate = useNavigate();
    const [exercise, setExercise] = useState<Exercise | null>(null);
    const [plannerExercises, setPlannerExercises] = useState<number[]>([]);

    const fetchExercise = async () => {
        try {
            const response = await axios.get<Exercise>(`${environment.apiUrl}Exercise/${exerciseId}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            setExercise(response.data);
        } catch (error) {
            console.error('Failed to fetch exercise:', error);
        }
    };

    const fetchPlanner = async () => {
        try {
            const date = new Date().toISOString().split('T')[0];
            const response = await axios.get(`${environment.apiUrl}Planner/user/${email}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                },
                params: { date }
            });
            const exerciseIds = response.data.exercises.map((exercise: Exercise) => exercise.id);
            setPlannerExercises(exerciseIds);
        } catch (error) {
            console.error('Failed to fetch planner:', error);
        }
    };

    useEffect(() => {
        fetchExercise();
        fetchPlanner();
    }, [exerciseId, token]);

    const handleAddToPlanner = async () => {
        if (!exercise) return;

        try {
            const date = new Date().toISOString();
            const response = await axios.post(`${environment.apiUrl}Planner`, {
                profile: email,
                date,
                mealIds: [],
                exerciseIds: [exercise.id]
            }, {
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`
                }
            });
            console.log('Added to planner:', response.data);
            setPlannerExercises([...plannerExercises, exercise.id]);
            notify('Exercise added to planner successfully', 'success');
        } catch (error) {
            console.error('Failed to add to planner:', error);
            notify('Failed to add exercise to planner', 'error');
        }
    };

    const isExerciseInPlanner = (exerciseId: number) => plannerExercises.includes(exerciseId);

    return (
        <Box sx={{ padding: 2 }}>
            {exercise ? (
                <Box>
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
                        <IconButton onClick={() => navigate(routes.exerciseListFull)} sx={{ color: '#00695c' }}>
                            <ArrowBackIcon />
                        </IconButton>
                        <IconButton
                            onClick={handleAddToPlanner}
                            sx={{
                                color: isExerciseInPlanner(exercise.id) ? '#ff4081' : '#ddd',
                                '&:hover': {
                                    color: isExerciseInPlanner(exercise.id) ? '#c60055' : '#ff4081',
                                },
                            }}
                        >
                            {isExerciseInPlanner(exercise.id) ? <FavoriteIcon /> : <FavoriteBorderIcon />}
                            Add Your Planner
                        </IconButton>
                    </Box>
                    <Typography id="exercise-modal-title" variant="h4" component="h2" align="center" sx={{ backgroundColor: '#00695c', color: '#fff', padding: '8px 0', marginBottom: '20px' }}>
                        {exercise.name}
                    </Typography>
                    {exercise.videoLink && (
                        <iframe
                            width="80%"
                            height="300"
                            src={exercise.videoLink.replace("watch?v=", "embed/")}
                            title={exercise.name}
                            allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                            allowFullScreen
                            style={{ display: 'block', margin: '0 auto', borderRadius: '10px' }}
                        ></iframe>
                    )}
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginTop: '20px' }}>
                        <Typography variant="h6" align="center">
                            <FaRunning color='#2a9d8f' /> {getTypeString(exercise.type)}
                        </Typography>
                        <Typography variant="h6" align="center">
                            <FaDumbbell color='#2a9d8f' /> {getEquipmentString(exercise.equipment)}
                        </Typography>
                    </Box>
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginTop: '20px' }}>
                        <Typography variant="h6" align="center">
                            Major Muscle: {getMajorMuscleString(exercise.majorMuscle)}
                        </Typography>
                    </Box>
                    <Typography variant="body1" paragraph sx={{ backgroundColor: '#e8f5e9', padding: '10px', borderRadius: '5px', marginTop: '20px' }}>
                        {exercise.description}
                    </Typography>
                </Box>
            ) : (
                <Typography variant="h6" align="center" sx={{ marginTop: 4, color: 'red' }}>
                    No exercise found.
                </Typography>
            )}
        </Box>
    );
};

export default ExerciseDetails;
