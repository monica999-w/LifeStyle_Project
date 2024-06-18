import React, { useState, ChangeEvent, FormEvent } from 'react';
import { RadioGroup, FormControlLabel, Radio, Button, Box, Typography, Divider } from '@mui/material';
import axios from 'axios';
import { useAuth } from '../../../components/provider/AuthProvider';
import { useNavigate } from 'react-router-dom';
import './ExerciseFilter.css';
import { environment } from '../../../environments/environment';  
import routes from '../../../components/route/routes';
import image from "../../../assets/image.png";

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

const exerciseTypes = [
    { value: 0, label: 'Yoga' },
    { value: 1, label: 'Cardio' },
    { value: 2, label: 'Pilates' },
    { value: 3, label: 'CrossFit' },
    { value: 4, label: 'Aerobics' },
];

const equipmentTypes = [
    { value: 0, label: 'BodyWeight' },
    { value: 1, label: 'Band' },
    { value: 2, label: 'Barbell' },
    { value: 3, label: 'Dumbbell' },
    { value: 4, label: 'Kettlebell' },
    { value: 5, label: 'Machine' },
    { value: 6, label: 'Medicine Ball' }
];

const majorMuscleGroups = [
    { value: 0, label: 'Arms' },
    { value: 1, label: 'Back' },
    { value: 2, label: 'Core' },
    { value: 3, label: 'Legs' },
    { value: 4, label: 'Shoulders' },
    { value: 5, label: 'Full Body' }
];

interface ExerciseFilterProps {
    setExercises: React.Dispatch<React.SetStateAction<Exercise[]>>;
}

const ExerciseFilter: React.FC<ExerciseFilterProps> = ({ setExercises }) => {
    const { token } = useAuth();
    const [type, setType] = useState<number | null>(null);
    const [equipment, setEquipment] = useState<number | null>(null);
    const [majorMuscle, setMajorMuscle] = useState<number | null>(null);
    const navigate = useNavigate();
    const apiUrl = `${environment.apiUrl}Exercise/filter`;

    const handleSubmit = async (event: FormEvent) => {
        event.preventDefault();
        console.log("API URL: ", apiUrl);
        const filter = {
            type,
            equipment,
            majorMuscle
        };

        try {
            const response = await axios.get<Exercise[]>(apiUrl, {
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`
                },
                params: filter
            });
            setExercises(response.data);
            navigate(routes.exerciseList);
        } catch (error) {
            console.error('Failed to fetch exercises:', error);
        }
    };

    return (
        <Box className="exercise-filter-container">
            <Box className="exercise-filter">
                <Typography variant="h4" component="h1" align="center" gutterBottom>
                    Workout Planner
                    <Divider />
                </Typography>
            
                <form onSubmit={handleSubmit} className="exercise-filter-form">
                    <Box className="exercise-content">
                        <Box className="exercise-left">
                            <Box className="exercise-section">
                                <Typography sx={{ fontWeight: 'bold',fontSize: '16px' }} >Choose exercise type:</Typography>
                                <RadioGroup
                                    value={type !== null ? type.toString() : ''}
                                    onChange={(e: ChangeEvent<HTMLInputElement>) => setType(Number(e.target.value))}
                                >
                                    {exerciseTypes.map((option) => (
                                        <FormControlLabel
                                            key={option.value}
                                            value={option.value.toString()}
                                            control={<Radio />}
                                            label={option.label}
                                            className="exercise-option"
                                        />
                                    ))}
                                </RadioGroup>
                            </Box>
                            <Box className="exercise-section">
                                <Typography sx={{ fontWeight: 'bold', fontSize: '16px'}}>Choose exercise equipment:</Typography>
                                <RadioGroup
                                    value={equipment !== null ? equipment.toString() : ''}
                                    onChange={(e: ChangeEvent<HTMLInputElement>) => setEquipment(Number(e.target.value))}
                                >
                                    {equipmentTypes.map((option) => (
                                        <FormControlLabel
                                            key={option.value}
                                            value={option.value.toString()}
                                            control={<Radio />}
                                            label={option.label}
                                            className="exercise-option"
                                        />
                                    ))}
                                </RadioGroup>
                            </Box>
                        </Box>
                        <Box className="exercise-right">
                            <img src={image} alt="Exercise" className="exercise-image" />
                            <Box className="exercise-section">
                                <Typography sx={{ fontWeight: 'bold',fontSize: '16px' }} >Choose major muscle:</Typography>
                                <RadioGroup
                                    value={majorMuscle !== null ? majorMuscle.toString() : ''}
                                    onChange={(e: ChangeEvent<HTMLInputElement>) => setMajorMuscle(Number(e.target.value))}
                                >
                                    {majorMuscleGroups.map((option) => (
                                        <FormControlLabel
                                            key={option.value}
                                            value={option.value.toString()}
                                            control={<Radio />}
                                            label={option.label}
                                            className="exercise-option"
                                        />
                                    ))}
                                </RadioGroup>
                            </Box>
                        </Box>
                    </Box>
                    <Box textAlign="center" mt={1}>
                        <Button
                            sx={{ 
                                width: '250px', 
                                height: '30px', 
                                fontSize: '16px', 
                                marginBottom: '20px' 
                            }}
                            type="submit" variant="contained" color="primary"
                        >
                            Submit
                        </Button>
                    </Box>
                </form>
            </Box>
        </Box>
    );
};

export default ExerciseFilter;
