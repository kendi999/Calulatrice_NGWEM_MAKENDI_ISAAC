namespace Calulatrice_NGWEM_MAKENDI_ISAAC;

public partial class MainPage : ContentPage
{
    // ===== État de la calculatrice =====
    private string _currentInput = "0";       // Ce que l'utilisateur tape actuellement
    private double _firstOperand = 0;          // Premier nombre de l'opération en cours
    private string? _pendingOperator = null;   // Opérateur en attente (+, -, ×, ÷)
    private bool _isNewInput = true;           // Faut-il remplacer l'affichage au prochain chiffre ?

    public MainPage()
    {
        InitializeComponent();
    }

    // ===== Boutons chiffres (0-9) =====
    private void OnDigitClicked(object sender, EventArgs e)
    {
        var digit = ((Button)sender).Text;

        if (_isNewInput)
        {
            _currentInput = digit;
            _isNewInput = false;
        }
        else
        {
            // Évite les zéros en tête (ex: "007" devient "7")
            if (_currentInput == "0")
                _currentInput = digit;
            else
                _currentInput += digit;
        }

        UpdateDisplay();
    }

    // ===== Bouton décimal (.) =====
    private void OnDecimalClicked(object sender, EventArgs e)
    {
        if (_isNewInput)
        {
            _currentInput = "0.";
            _isNewInput = false;
        }
        else if (!_currentInput.Contains('.'))
        {
            _currentInput += ".";
        }

        UpdateDisplay();
    }

    // ===== Boutons opérateurs (+, -, ×, ÷) =====
    private void OnOperatorClicked(object sender, EventArgs e)
    {
        var op = ((Button)sender).Text;

        // Si une opération est déjà en attente, on la calcule d'abord
        if (_pendingOperator != null && !_isNewInput)
        {
            Compute();
        }

        _firstOperand = double.Parse(_currentInput);
        _pendingOperator = op;
        _isNewInput = true;
        UpdateDisplay();
    }

    // ===== Bouton égal (=) =====
    private void OnEqualsClicked(object sender, EventArgs e)
    {
        if (_pendingOperator == null) return;

        Compute();
        _pendingOperator = null;
        _isNewInput = true;
        HistoryLabel.Text = string.Empty;
    }

    // ===== Bouton Clear (C) =====
    private void OnClearClicked(object sender, EventArgs e)
    {
        _currentInput = "0";
        _firstOperand = 0;
        _pendingOperator = null;
        _isNewInput = true;
        HistoryLabel.Text = string.Empty;
        UpdateDisplay();
    }

    // ===== Bouton signe (±) =====
    private void OnSignClicked(object sender, EventArgs e)
    {
        if (_currentInput.StartsWith('-'))
            _currentInput = _currentInput[1..];
        else if (_currentInput != "0")
            _currentInput = "-" + _currentInput;

        UpdateDisplay();
    }

    // ===== Bouton pourcentage (%) =====
    private void OnPercentClicked(object sender, EventArgs e)
    {
        var value = double.Parse(_currentInput) / 100;
        _currentInput = value.ToString();
        UpdateDisplay();
    }

    // ===== Méthode utilitaire : calcule l'opération en attente =====
    private void Compute()
    {
        var second = double.Parse(_currentInput);
        double result = _pendingOperator switch
        {
            "+" => _firstOperand + second,
            "-" => _firstOperand - second,
            "×" => _firstOperand * second,
            "÷" => second == 0 ? 0 : _firstOperand / second,
            _   => second
        };

        _currentInput = result.ToString();
        _firstOperand = result;
        UpdateDisplay();
    }

    // ===== Méthode utilitaire : met à jour l'affichage =====
    private void UpdateDisplay()
    {
        DisplayLabel.Text = _currentInput;

        if (_pendingOperator != null)
            HistoryLabel.Text = $"{_firstOperand} {_pendingOperator}";
        else
            HistoryLabel.Text = string.Empty;
    }
}