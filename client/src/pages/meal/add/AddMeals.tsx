import React, { useState } from 'react';
import { TextField, Button, Box, Typography, MenuItem, InputLabel, Select, FormControl } from '@mui/material';
import axios from 'axios';
import { useAuth } from '../../../components/provider/AuthProvider';
import { useNotification } from '../../../components/provider/NotificationContext';
import { environment } from '../../../environments/environment';  

const mealTypes = [
    { value: 0, label: 'Breakfast' },
    { value: 1, label: 'Lunch' },
    { value: 2, label: 'Dinner' },
];

const allergyTypes = [
    { value: 0, label: 'Dairy' },
    { value: 1, label: 'Egg' },
    { value: 2, label: 'Gluten' },
    { value: 3, label: 'Peanut' },
    { value: 4, label: 'Seafood' },
    { value: 5, label: 'Sesame' },
    { value: 6, label: 'Shellfish' },
    { value: 7, label: 'Soy' },
    { value: 8, label: 'TreeNut' },
    { value: 9, label: 'Wheat' },
    { value: 10, label: 'Corn' },
];

const dietTypes = [
    { value: 0, label: 'NoDiet' },
    { value: 1, label: 'LactoVegetarian' },
    { value: 2, label: 'OvoVegetarian' },
    { value: 3, label: 'Paleo' },
    { value: 4, label: 'Primal' },
    { value: 5, label: 'Pescetarian' },
    { value: 6, label: 'Vegan' },
    { value: 7, label: 'Vegetarian' },
    { value: 8, label: 'Whole30' },
    { value: 9, label: 'Vegetarian' },
];

const CreateMeal = () => {
    const { token } = useAuth();
    const { notify } = useNotification();
    const [name, setName] = useState('');
    const [mealType, setMealType] = useState<number | ''>('');
    const [calories, setCalories] = useState('');
    const [protein, setProtein] = useState('');
    const [carbohydrates, setCarbohydrates] = useState('');
    const [fat, setFat] = useState('');
    const [allergies, setAllergies] = useState<number[]>([]);
    const [diets, setDiets] = useState<number[]>([]);
    const [ingredients, setIngredients] = useState<string[]>(['']);
    const [preparationInstructions, setPreparationInstructions] = useState('');
    const [estimatedPreparationTime, setEstimatedPreparationTime] = useState('');
    const [image, setImage] = useState<File | null>(null);
    const [error, setError] = useState('');

    const handleIngredientChange = (index: number, value: string) => {
        const newIngredients = [...ingredients];
        newIngredients[index] = value;
        setIngredients(newIngredients);
    };

    const addIngredientField = () => {
        setIngredients([...ingredients, '']);
    };

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.files && event.target.files.length > 0) {
            setImage(event.target.files[0]);
        }
    };

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
    
        if (!name.trim()) {
            setError('Meal name cannot be empty');
            return;
        }
    
        const formData = new FormData();
        formData.append('MealName', name);
        formData.append('mealType', mealType.toString());
        formData.append('nutrients.calories', calories);
        formData.append('nutrients.protein', protein);
        formData.append('nutrients.carbohydrates', carbohydrates);
        formData.append('nutrients.fat', fat);
        formData.append('preparationInstructions', preparationInstructions);
        formData.append('estimatedPreparationTimeInMinutes', estimatedPreparationTime);
    
        ingredients.forEach((ingredient, index) => {
            formData.append(`ingredients[${index}]`, ingredient);
        });
    
        allergies.forEach((allergy, index) => {
            formData.append(`allergies[${index}]`, allergy.toString());
        });
    
        diets.forEach((diet, index) => {
            formData.append(`diets[${index}]`, diet.toString());
        });
    
        if (image) {
            formData.append('imageUrl', image);
        }
    
        try {
            await axios.post(`${environment.apiUrl}Meal`, formData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                    Authorization: `Bearer ${token}`
                }
            });
            setError('');
            notify('Meal created successfully!', 'success');
            // Reset form fields
            setName('');
            setMealType('');
            setCalories('');
            setProtein('');
            setCarbohydrates('');
            setFat('');
            setAllergies([]);
            setDiets([]);
            setIngredients(['']);
            setPreparationInstructions('');
            setEstimatedPreparationTime('');
            setImage(null);
        } catch (error: any) {
            setError('Failed to create meal');
            notify('Failed to create meal', 'error');
            console.error(error.response?.data || error.message);
        }
    };

    return (
        <Box>
            <Typography variant="h4" gutterBottom>Create Meal</Typography>
            <form onSubmit={handleSubmit}>
                <TextField
                    label="Name"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    fullWidth
                    margin="normal"
                    required
                />
                <FormControl fullWidth margin="normal" required>
                    <InputLabel>Meal Type</InputLabel>
                    <Select
                        value={mealType}
                        onChange={(e) => setMealType(Number(e.target.value))}
                    >
                        {mealTypes.map((option) => (
                            <MenuItem key={option.value} value={option.value}>
                                {option.label}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>
                <TextField
                    label="Calories"
                    value={calories}
                    onChange={(e) => setCalories(e.target.value)}
                    fullWidth
                    margin="normal"
                    required
                />
                <TextField
                    label="Protein (g)"
                    value={protein}
                    onChange={(e) => setProtein(e.target.value)}
                    fullWidth
                    margin="normal"
                    required
                />
                <TextField
                    label="Carbohydrates (g)"
                    value={carbohydrates}
                    onChange={(e) => setCarbohydrates(e.target.value)}
                    fullWidth
                    margin="normal"
                    required
                />
                <TextField
                    label="Fat (g)"
                    value={fat}
                    onChange={(e) => setFat(e.target.value)}
                    fullWidth
                    margin="normal"
                    required
                />
                <Typography variant="h6" gutterBottom>Ingredients</Typography>
                {ingredients.map((ingredient, index) => (
                    <TextField
                        key={index}
                        label={`Ingredient ${index + 1}`}
                        value={ingredient}
                        onChange={(e) => handleIngredientChange(index, e.target.value)}
                        fullWidth
                        margin="normal"
                        required
                    />
                ))}
                <Button onClick={addIngredientField}>Add Ingredient</Button>
                <FormControl fullWidth margin="normal" required>
                    <InputLabel>Allergies</InputLabel>
                    <Select
                        multiple
                        value={allergies}
                        onChange={(e) => setAllergies(e.target.value as number[])}
                    >
                        {allergyTypes.map((option) => (
                            <MenuItem key={option.value} value={option.value}>
                                {option.label}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>
                <FormControl fullWidth margin="normal" required>
                    <InputLabel>Diets</InputLabel>
                    <Select
                        multiple
                        value={diets}
                        onChange={(e) => setDiets(e.target.value as number[])}
                    >
                        {dietTypes.map((option) => (
                            <MenuItem key={option.value} value={option.value}>
                                {option.label}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>
                <TextField
                    label="Preparation Instructions"
                    value={preparationInstructions}
                    onChange={(e) => setPreparationInstructions(e.target.value)}
                    fullWidth
                    margin="normal"
                    required
                    multiline
                    rows={4}
                />
                <TextField
                    label="Estimated Preparation Time (minutes)"
                    value={estimatedPreparationTime}
                    onChange={(e) => setEstimatedPreparationTime(e.target.value)}
                    fullWidth
                    margin="normal"
                    required
                />
                <input
                    type="file"
                    accept="image/*"
                    onChange={handleFileChange}
                    style={{ display: 'block', marginTop: '10px' }}
                />
                {error && <Typography color="error" gutterBottom>{error}</Typography>}
                <Button type="submit" variant="contained" color="primary" fullWidth>
                    Create Meal
                </Button>
            </form>
        </Box>
    );
};

export default CreateMeal;
